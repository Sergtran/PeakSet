using PeakSet.Application.Abstractions;
using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Application.Dtos;
using PeakSet.Domain.Enums;

namespace PeakSet.Application.Services;

public sealed class ExerciseStatsService : IExerciseStatsService
{
	private readonly IWorkoutRepository _workoutRepository;

	public ExerciseStatsService(IWorkoutRepository workoutRepository)
	{
		_workoutRepository = workoutRepository;
	}

	public async Task<IReadOnlyList<ExerciseSummaryDto>> GetExercisesAsync(
		string userId, CancellationToken ct = default)
	{
		var exercises = await _workoutRepository.GetUserExercisesAsync(userId, ct);

		return exercises
			.Select(e => new ExerciseSummaryDto(e.Name, e.SessionCount, e.LastUsed))
			.ToList();
	}

	public async Task<ExerciseProgressDto?> GetProgressAsync(
		string userId, string exerciseName, CancellationToken ct = default)
	{
		var sessions = await _workoutRepository.GetExerciseSessionRowsAsync(userId, exerciseName, ct);
		if (sessions.Count == 0)
			return null;

		var ordered = sessions
			.Select(s => (Session: s, Best: BestSet(s.Sets, s.ExerciseType)))
			.OrderBy(x => x.Session.WorkoutDate)
			.ToList();

		var allSets = sessions.SelectMany(s => s.Sets).ToList();

		return new ExerciseProgressDto(
			exerciseName,
			sessions.Count,
			allSets.Count,
			allSets.Count > 0 ? allSets.Max(s => s.Weight) : null,
			allSets.Count > 0 ? allSets.Max(s => s.Reps) : null,
			ordered.First().Session.WorkoutDate,
			ordered.Last().Session.WorkoutDate,
			ordered
				.Select(p => new ProgressPointDto(
					p.Session.WorkoutDate,
					p.Best.Weight,
					p.Best.Reps,
					p.Session.PrStatus))
				.ToList());
	}

	private static (decimal? Weight, int? Reps) BestSet(
		IReadOnlyList<(int Reps, decimal Weight)> sets, ExerciseType type)
	{
		if (sets.Count == 0)
			return (null, null);

		// Bodyweight: la métrica es reps. Resto: peso (en time, weight = segundos de trabajo);
		// si hay empate de peso, gana el que más reps hizo.
		if (type == ExerciseType.Bodyweight)
		{
			var maxReps = sets.Max(s => s.Reps);
			return (null, maxReps);
		}

		var best = sets.OrderByDescending(s => s.Weight).ThenByDescending(s => s.Reps).First();
		return (best.Weight, best.Reps);
	}
}
