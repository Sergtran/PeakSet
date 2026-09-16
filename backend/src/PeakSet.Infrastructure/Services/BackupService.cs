using PeakSet.Application.Abstractions;
using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Application.Dtos;
using PeakSet.Application.Exceptions;
using PeakSet.Domain.Entities;
using PeakSet.Domain.Enums;
using PeakSet.Domain.ValueObjects;
using PeakSet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PeakSet.Infrastructure.Services;

public sealed class BackupService : IBackupService
{
	private readonly PeakSetDbContext _db;
	private readonly IRoutineRepository _routineRepository;
	private readonly IWorkoutRepository _workoutRepository;
	private readonly IUserSettingsRepository _settingsRepository;

	public BackupService(
		PeakSetDbContext db,
		IRoutineRepository routineRepository,
		IWorkoutRepository workoutRepository,
		IUserSettingsRepository settingsRepository)
	{
		_db = db;
		_routineRepository = routineRepository;
		_workoutRepository = workoutRepository;
		_settingsRepository = settingsRepository;
	}

	public async Task<PeakSetExportDto> ExportAsync(string userId, CancellationToken ct = default)
	{
		var routines = await _routineRepository.GetByUserWithSessionsAsync(userId, ct);
		var workouts = await _workoutRepository.GetByUserAllAsync(userId, ct);
		var settings = await _settingsRepository.GetByUserIdAsync(userId, ct);

		return new PeakSetExportDto(
			Version: 1,
			ExportedAtUtc: DateTime.UtcNow,
			Settings: MapSettings(settings),
			Routines: routines.Select(MapRoutine).ToList(),
			Workouts: workouts.OrderBy(w => w.WorkoutDate).Select(MapWorkout).ToList());
	}

	public async Task ImportAsync(string userId, PeakSetExportDto snapshot, CancellationToken ct = default)
	{
		if (snapshot.Version != 1)
			throw new ValidationException(new[]
			{
				new ValidationError("UnsupportedBackupVersion", "Unsupported backup version.")
			});

		var routines = snapshot.Routines ?? Array.Empty<RoutineExportDto>();
		var workouts = snapshot.Workouts ?? Array.Empty<WorkoutExportDto>();

		// A duplicate routine Id would break the remapping: validate before touching the database.
		var routineIds = new HashSet<Guid>();
		foreach (var routine in routines)
			if (!routineIds.Add(routine.Id))
			throw new ValidationException(new[]
			{
				new ValidationError("DuplicateRoutineIds", "The backup contains routines with duplicate Ids.")
			});

		await using var transaction = await _db.Database.BeginTransactionAsync(ct);
		try
		{
			// 1) Replace: delete the user workouts and routines (children via CASCADE in the database).
			//    Per-exercise notes/preferences are preserved: they are tied to the name, not the Id.
			await _db.Database.ExecuteSqlRawAsync(
				"DELETE FROM \"Workouts\" WHERE \"UserId\" = {0}; "
				+ "DELETE FROM \"Routines\" WHERE \"UserId\" = {0};",
				new object[] { userId },
				cancellationToken: ct);

			// 2) Se reconstruye con Ids NUEVOS y se remapea RoutineId viejo -> nuevo.
			var routineIdMap = new Dictionary<Guid, Guid>();
			foreach (var routineDto in routines)
			{
				var routine = new Routine(userId, new Name(routineDto.Name));
				foreach (var sessionDto in routineDto.Sessions)
				{
					var session = new WorkoutSession(new Name(sessionDto.Name));
					foreach (var exerciseDto in sessionDto.Exercises)
					{
						session.AddExercise(
							new Name(exerciseDto.Name),
							exerciseDto.ExerciseType,
							exerciseDto.Laterality);
					}
					routine.AddSession(session);
				}
				await _routineRepository.AddAsync(routine, ct);
				routineIdMap[routineDto.Id] = routine.Id;
			}

			// 3) Workouts: date, snapshots and historical PrStatus are preserved (not recomputed).
			foreach (var workoutDto in workouts)
			{
				var mappedRoutineId = workoutDto.RoutineId.HasValue
					&& routineIdMap.TryGetValue(workoutDto.RoutineId.Value, out var mapped)
						? mapped
						: (Guid?)null;

				var workout = new Workout(
					userId,
					new Name(workoutDto.RoutineName),
					new Name(workoutDto.SessionName),
					workoutDto.WorkoutDate,
					mappedRoutineId);

				foreach (var exerciseDto in workoutDto.Exercises)
				{
					var exercise = workout.AddExercise(
						new Name(exerciseDto.Name),
						exerciseDto.ExerciseType,
						exerciseDto.Laterality);

					foreach (var setDto in exerciseDto.Sets)
						exercise.AddSet(new Repetitions(setDto.Reps), new Weight(setDto.Weight));

					exercise.SetPrStatus(exerciseDto.PrStatus);
				}
				await _workoutRepository.AddAsync(workout, ct);
			}

			// 4) Settings: applied (remapping the current routine when present in the backup).
			var existing = await _settingsRepository.GetByUserIdAsync(userId, ct);
			var created = existing is null;
			var settings = existing ?? new UserSettings(userId);

			settings.SetTheme(snapshot.Settings.Theme);
			settings.UpdateTimer(
				snapshot.Settings.TimerPrepSeconds,
				snapshot.Settings.TimerWorkSeconds,
				snapshot.Settings.TimerRestSeconds,
				snapshot.Settings.TimerSets);

			var currentRoutineId = snapshot.Settings.CurrentRoutineId;
			if (currentRoutineId.HasValue && routineIdMap.TryGetValue(currentRoutineId.Value, out var newCurrentId))
				settings.SetCurrentRoutine(newCurrentId);
			else
				settings.ClearCurrentRoutine();

			if (created)
				await _settingsRepository.AddAsync(settings, ct);
			else
				await _settingsRepository.UpdateAsync(settings, ct);

			await transaction.CommitAsync(ct);
		}
		catch
		{
			await transaction.RollbackAsync(ct);
			throw;
		}
	}

	private static UserSettingsExportDto MapSettings(UserSettings? settings)
		=> new(
			settings?.Theme ?? Theme.Light,
			settings?.TimerPrepSeconds ?? UserSettings.TimerDefaults.PrepSeconds,
			settings?.TimerWorkSeconds ?? UserSettings.TimerDefaults.WorkSeconds,
			settings?.TimerRestSeconds ?? UserSettings.TimerDefaults.RestSeconds,
			settings?.TimerSets ?? UserSettings.TimerDefaults.Sets,
			settings?.CurrentRoutineId);

	private static RoutineExportDto MapRoutine(Routine routine)
		=> new(
			routine.Id,
			routine.Name.Value,
			routine.Sessions.OrderBy(s => s.DisplayOrder)
				.Select(s => new SessionExportDto(
					s.Id,
					s.Name.Value,
					s.DisplayOrder,
					s.Exercises.OrderBy(e => e.DisplayOrder)
						.Select(e => new SessionExerciseExportDto(
							e.Id, e.Name.Value, e.ExerciseType, e.Laterality, e.DisplayOrder))
						.ToList()))
				.ToList());

	private static WorkoutExportDto MapWorkout(Workout workout)
		=> new(
			workout.Id,
			workout.RoutineId,
			workout.RoutineName.Value,
			workout.SessionName.Value,
			workout.WorkoutDate,
			workout.Exercises.OrderBy(e => e.DisplayOrder)
				.Select(e => new WorkoutExerciseExportDto(
					e.Id,
					e.Name.Value,
					e.ExerciseType,
					e.Laterality,
					e.PrStatus,
					e.DisplayOrder,
					e.Sets.OrderBy(s => s.SetNumber)
						.Select(s => new WorkoutSetExportDto(
							s.Id, s.SetNumber, s.Reps.Value, s.Weight.Value))
						.ToList()))
				.ToList());
}
