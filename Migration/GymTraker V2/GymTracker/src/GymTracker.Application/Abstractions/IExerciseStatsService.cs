using GymTracker.Application.Dtos;

namespace GymTracker.Application.Abstractions;

public interface IExerciseStatsService
{
	Task<IReadOnlyList<ExerciseSummaryDto>> GetExercisesAsync(
		string userId,
		CancellationToken ct = default);

	Task<ExerciseProgressDto?> GetProgressAsync(
		string userId,
		string exerciseName,
		CancellationToken ct = default);
}