using GymTracker.Application.Abstractions;
using GymTracker.Application.Abstractions.Repositories;
using GymTracker.Application.Dtos;
using GymTracker.Application.Exceptions;
using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;
using GymTracker.Domain.ValueObjects;

namespace GymTracker.Application.Services;

public sealed class WorkoutService : IWorkoutService
{
	private readonly IWorkoutRepository _repository;

	public WorkoutService(IWorkoutRepository repository)
		=> _repository = repository;

	public async Task<WorkoutDto> CreateWorkoutAsync(
		string userId, CreateWorkoutRequest request, CancellationToken ct = default)
	{
		var workout = new Workout(
			userId,
			new Name(request.RoutineName),
			new Name(request.SessionName),
			request.WorkoutDate,
			request.RoutineId);

		foreach (var exerciseRequest in request.Exercises)
		{
			var exercise = workout.AddExercise(
				new Name(exerciseRequest.Name),
				exerciseRequest.ExerciseType,
				exerciseRequest.Laterality);

			foreach (var setRequest in exerciseRequest.Sets)
				exercise.AddSet(new Repetitions(setRequest.Reps), new Weight(setRequest.Weight));

			var currentMax = MaxMetric(exercise);
			var previousBest = await _repository.GetPreviousBestMetricAsync(
				userId, exerciseRequest.Name, exerciseRequest.Laterality,
				exerciseRequest.ExerciseType, null, ct);

			exercise.SetPrStatus(ResolvePrStatus(currentMax, previousBest));
		}

		await _repository.AddAsync(workout, ct);
		return MapWorkout(workout);
	}

	public async Task<PagedResult<WorkoutDto>> GetWorkoutsAsync(
		string userId, int page, int pageSize, CancellationToken ct = default)
	{
		var safePage = Math.Max(1, page);
		var safePageSize = Math.Clamp(pageSize, 1, 100);

		var (items, totalCount) = await _repository.GetByUserPagedAsync(
			userId, safePage, safePageSize, ct);

		return new PagedResult<WorkoutDto>(
			items.Select(MapWorkout).ToList(),
			safePage,
			safePageSize,
			totalCount,
			(int)Math.Ceiling(totalCount / (double)safePageSize));
	}

	public async Task<WorkoutDto> GetWorkoutAsync(
		string userId, Guid id, CancellationToken ct = default)
	{
		var workout = await _repository.GetByIdAsync(userId, id, ct)
			?? throw new NotFoundException("Entrenamiento no encontrado.");

		return MapWorkout(workout);
	}

	public async Task<WorkoutDto> EditWorkoutAsync(
		string userId, Guid id, CreateWorkoutRequest request, CancellationToken ct = default)
	{
		// Editar = borrar el anterior y crear uno nuevo. Reemplazar hijos en EF con CASCADE
		// resultaba en "expected 1 row, affected 0"; este flujo es simple y correcto:
		// al borrar primero, el PR del nuevo se calcula contra el resto del historial.
		var tracked = await _repository.GetByIdForUpdateAsync(userId, id, ct)
			?? throw new NotFoundException("Entrenamiento no encontrado.");

		await _repository.DeleteAsync(tracked, ct);

		var workout = new Workout(
			userId,
			new Name(request.RoutineName),
			new Name(request.SessionName),
			request.WorkoutDate,
			request.RoutineId);

		foreach (var exerciseRequest in request.Exercises)
		{
			var exercise = workout.AddExercise(
				new Name(exerciseRequest.Name),
				exerciseRequest.ExerciseType,
				exerciseRequest.Laterality);

			foreach (var setRequest in exerciseRequest.Sets)
				exercise.AddSet(new Repetitions(setRequest.Reps), new Weight(setRequest.Weight));

			var currentMax = MaxMetric(exercise);
			var previousBest = await _repository.GetPreviousBestMetricAsync(
				userId, exerciseRequest.Name, exerciseRequest.Laterality,
				exerciseRequest.ExerciseType, null, ct);

			exercise.SetPrStatus(ResolvePrStatus(currentMax, previousBest));
		}

		await _repository.AddAsync(workout, ct);
		return MapWorkout(workout);
	}

	public async Task DeleteWorkoutAsync(string userId, Guid id, CancellationToken ct = default)
	{
		var workout = await _repository.GetByIdForUpdateAsync(userId, id, ct)
			?? throw new NotFoundException("Entrenamiento no encontrado.");

		await _repository.DeleteAsync(workout, ct);
	}

	private static decimal MaxMetric(WorkoutExercise exercise)
		=> exercise.ExerciseType == ExerciseType.Bodyweight
			? exercise.Sets.Max(s => (decimal)s.Reps.Value)
			: exercise.Sets.Max(s => s.Weight.Value);

	private static PrStatus? ResolvePrStatus(decimal currentMax, decimal? previousBest)
	{
		var best = previousBest ?? 0m;

		if (currentMax > best)
			return PrStatus.New;

		if (currentMax == best && best > 0)
			return PrStatus.Matched;

		return null;
	}

	private static WorkoutDto MapWorkout(Workout workout)
		=> new(
			workout.Id,
			workout.RoutineId,
			workout.RoutineName.Value,
			workout.SessionName.Value,
			workout.WorkoutDate,
			workout.CreatedAt,
			workout.Exercises.OrderBy(e => e.DisplayOrder)
				.Select(e => new WorkoutExerciseDto(
					e.Id,
					e.Name.Value,
					e.ExerciseType,
					e.Laterality,
					e.PrStatus,
					e.Sets.OrderBy(s => s.SetNumber)
						.Select(s => new WorkoutSetDto(s.SetNumber, s.Reps.Value, s.Weight.Value))
						.ToList()))
				.ToList());
}
