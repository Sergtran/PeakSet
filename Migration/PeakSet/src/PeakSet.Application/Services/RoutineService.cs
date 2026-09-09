using PeakSet.Application.Abstractions;
using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Application.Dtos;
using PeakSet.Application.Exceptions;
using PeakSet.Domain.Entities;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Application.Services;

public sealed class RoutineService : IRoutineService
{
	private readonly IRoutineRepository _repository;

	public RoutineService(IRoutineRepository repository)
		=> _repository = repository;

	public async Task<RoutineDto> CreateRoutineAsync(
		string userId, CreateRoutineRequest request, CancellationToken ct = default)
	{
		var name = await GetUniqueNameAsync(userId, request.Name, null, ct);

		var routine = new Routine(userId, new Name(name));
		await _repository.AddAsync(routine, ct);

		return new RoutineDto(routine.Id, routine.Name.Value, routine.CreatedAt);
	}

	public async Task<IReadOnlyList<RoutineDto>> GetRoutinesAsync(
		string userId, CancellationToken ct = default)
	{
		var routines = await _repository.GetByUserAsync(userId, ct);

		return routines
			.OrderBy(r => r.CreatedAt)
			.Select(r => new RoutineDto(r.Id, r.Name.Value, r.CreatedAt))
			.ToList();
	}

	public async Task<RoutineDto> GetRoutineAsync(
		string userId, Guid id, CancellationToken ct = default)
	{
		var routine = await _repository.GetByIdAsync(userId, id, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

		return new RoutineDto(routine.Id, routine.Name.Value, routine.CreatedAt);
	}

	private async Task<string> GetUniqueNameAsync(
		string userId, string baseName, Guid? excludeId, CancellationToken ct)
	{
		if (!await _repository.ExistsByNameAsync(userId, baseName, excludeId, ct))
			return baseName;

		for (var i = 1; ; i++)
		{
			var candidate = $"{baseName} ({i})";
			if (!await _repository.ExistsByNameAsync(userId, candidate, excludeId, ct))
				return candidate;
		}
	}

	public async Task<SessionDto> AddSessionAsync(
		string userId, Guid routineId, CreateSessionRequest request, CancellationToken ct = default)
	{
		var routine = await _repository.GetByIdWithSessionsAsync(userId, routineId, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

		routine.AddSession(new WorkoutSession(new Name(request.Name)));
		await _repository.UpdateAsync(routine, ct);

		var session = routine.Sessions.Last();
		return MapSession(session);
	}

	public async Task<SessionExerciseDto> AddExerciseAsync(
		string userId, Guid routineId, Guid sessionId,
		CreateSessionExerciseRequest request, CancellationToken ct = default)
	{
		var routine = await _repository.GetByIdWithSessionsAsync(userId, routineId, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

		var session = routine.Sessions.FirstOrDefault(s => s.Id == sessionId)
			?? throw new NotFoundException("Sesión no encontrada.");

		session.AddExercise(
			new Name(request.Name), request.ExerciseType, request.Laterality);
		await _repository.UpdateAsync(routine, ct);

		var exercise = session.Exercises.Last();
		return new SessionExerciseDto(
			exercise.Id, exercise.Name.Value, exercise.ExerciseType,
			exercise.Laterality, exercise.DisplayOrder);
	}

	public async Task<RoutineDetailDto> GetRoutineDetailAsync(
		string userId, Guid id, CancellationToken ct = default)
	{
		var routine = await _repository.GetByIdWithSessionsAsync(userId, id, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

		return new RoutineDetailDto(
			routine.Id, routine.Name.Value, routine.CreatedAt,
			routine.Sessions.OrderBy(s => s.DisplayOrder)
				.Select(MapSession).ToList());
	}

	public async Task<RoutineDto> RenameRoutineAsync(
		string userId, Guid id, CreateRoutineRequest request, CancellationToken ct = default)
	{
		var routine = await _repository.GetByIdAsync(userId, id, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

		var name = await GetUniqueNameAsync(userId, request.Name, routine.Id, ct);
		routine.Rename(new Name(name));
		await _repository.UpdateAsync(routine, ct);

		return new RoutineDto(routine.Id, routine.Name.Value, routine.CreatedAt);
	}

	public async Task<SessionDto> RenameSessionAsync(
		string userId, Guid routineId, Guid sessionId,
		CreateSessionRequest request, CancellationToken ct = default)
	{
		var routine = await GetRoutineWithSessionsAsync(userId, routineId, ct);
		var session = FindSession(routine, sessionId);

		session.Rename(new Name(request.Name));
		await _repository.UpdateAsync(routine, ct);

		return MapSession(session);
	}

	public async Task<SessionExerciseDto> EditExerciseAsync(
		string userId, Guid routineId, Guid sessionId, Guid exerciseId,
		CreateSessionExerciseRequest request, CancellationToken ct = default)
	{
		var routine = await GetRoutineWithSessionsAsync(userId, routineId, ct);
		var session = FindSession(routine, sessionId);
		var exercise = FindExercise(session, exerciseId);

		exercise.Rename(new Name(request.Name));
		exercise.SetExerciseType(request.ExerciseType);
		exercise.SetLaterality(request.Laterality);
		await _repository.UpdateAsync(routine, ct);

		return new SessionExerciseDto(
			exercise.Id, exercise.Name.Value, exercise.ExerciseType,
			exercise.Laterality, exercise.DisplayOrder);
	}

	public async Task DeleteSessionAsync(
		string userId, Guid routineId, Guid sessionId, CancellationToken ct = default)
	{
		var routine = await GetRoutineWithSessionsAsync(userId, routineId, ct);
		FindSession(routine, sessionId);

		routine.RemoveSession(sessionId);
		await _repository.UpdateAsync(routine, ct);
	}

	public async Task DeleteExerciseAsync(
		string userId, Guid routineId, Guid sessionId, Guid exerciseId, CancellationToken ct = default)
	{
		var routine = await GetRoutineWithSessionsAsync(userId, routineId, ct);
		var session = FindSession(routine, sessionId);
		FindExercise(session, exerciseId);

		session.RemoveExercise(exerciseId);
		await _repository.UpdateAsync(routine, ct);
	}

	public async Task DeleteRoutineAsync(string userId, Guid id, CancellationToken ct = default)
	{
		var routine = await _repository.GetByIdAsync(userId, id, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

		await _repository.DeleteAsync(routine, ct);
	}

	private async Task<Routine> GetRoutineWithSessionsAsync(
		string userId, Guid routineId, CancellationToken ct)
		=> await _repository.GetByIdWithSessionsAsync(userId, routineId, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

	private static WorkoutSession FindSession(Routine routine, Guid sessionId)
		=> routine.Sessions.FirstOrDefault(s => s.Id == sessionId)
			?? throw new NotFoundException("Sesión no encontrada.");

	private static SessionExercise FindExercise(WorkoutSession session, Guid exerciseId)
		=> session.Exercises.FirstOrDefault(e => e.Id == exerciseId)
			?? throw new NotFoundException("Ejercicio no encontrado.");

	private static SessionDto MapSession(WorkoutSession session)
		=> new(
			session.Id, session.Name.Value, session.DisplayOrder,
			session.Exercises.OrderBy(e => e.DisplayOrder)
				.Select(e => new SessionExerciseDto(
					e.Id, e.Name.Value, e.ExerciseType, e.Laterality, e.DisplayOrder))
				.ToList());
}
