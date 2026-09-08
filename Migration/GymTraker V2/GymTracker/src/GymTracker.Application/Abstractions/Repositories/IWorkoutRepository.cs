using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;
using GymTracker.Application.Dtos;

namespace GymTracker.Application.Abstractions.Repositories;

public interface IWorkoutRepository
{
	Task<Workout?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default);
	Task<(IReadOnlyList<Workout> Items, int TotalCount)> GetByUserPagedAsync(
		string userId, int page, int pageSize, CancellationToken ct = default);
	Task<decimal?> GetPreviousBestMetricAsync(
		string userId, string exerciseName, Laterality laterality,
		ExerciseType type, Guid? excludeWorkoutId = null, CancellationToken ct = default);
	Task<RoutineWorkoutSummary?> GetRoutineSummaryAsync(string userId, Guid routineId, CancellationToken ct = default);
	Task<IReadOnlyList<DateTime>> GetWorkoutDatesAsync(string userId, Guid routineId, CancellationToken ct = default);
	Task<IReadOnlyList<ExerciseFrequency>> GetExerciseFrequencyAsync(
		string userId, Guid routineId, int limit, CancellationToken ct = default);
	Task<IReadOnlyList<ExerciseSummary>> GetUserExercisesAsync(
		string userId, CancellationToken ct = default);

	Task<IReadOnlyList<ExerciseSessionRow>> GetExerciseSessionRowsAsync(
		string userId,
		string exerciseName,
		CancellationToken ct = default);
	Task<IReadOnlyList<CalendarWorkoutRow>> GetCalendarWorkoutsAsync(
		string userId, DateTime fromInclusive, DateTime toExclusive, CancellationToken ct = default);
	Task<Workout?> GetByIdForUpdateAsync(string userId, Guid id, CancellationToken ct = default);
	Task UpdateAsync(Workout workout, CancellationToken ct = default);
	Task DeleteAsync(Workout workout, CancellationToken ct = default);
	Task AddAsync(Workout workout, CancellationToken ct = default);

	Task<IReadOnlyList<Workout>> GetByUserAllAsync(string userId, CancellationToken ct = default);
}
