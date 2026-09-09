using PeakSet.Application.Abstractions;
using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Application.Dtos;
using PeakSet.Application.Exceptions;
using PeakSet.Domain.Entities;

namespace PeakSet.Application.Services;

public sealed class RoutineStatsService : IRoutineStatsService
{
	private const int UsageGapDays = 14;
	private const int ConsistencyWeeks = 8;

	private readonly IUserSettingsRepository _settingsRepository;
	private readonly IRoutineRepository _routineRepository;
	private readonly IWorkoutRepository _workoutRepository;

	public RoutineStatsService(
		IUserSettingsRepository settingsRepository,
		IRoutineRepository routineRepository,
		IWorkoutRepository workoutRepository)
	{
		_settingsRepository = settingsRepository;
		_routineRepository = routineRepository;
		_workoutRepository = workoutRepository;
	}

	public async Task<HomeDto> GetHomeAsync(string userId, CancellationToken ct = default)
	{
		var settings = await _settingsRepository.GetByUserIdAsync(userId, ct);
		if (settings?.CurrentRoutineId is not Guid routineId)
			return new HomeDto(null);

		var routine = await _routineRepository.GetByIdAsync(userId, routineId, ct);
		if (routine is null)
			return new HomeDto(null);

		var summary = await _workoutRepository.GetRoutineSummaryAsync(userId, routineId, ct);
		return new HomeDto(BuildOverview(routine.Id, routine.Name.Value, summary));
	}

	public async Task<RoutineStatsDto> GetRoutineStatsAsync(
		string userId, Guid routineId, CancellationToken ct = default)
	{
		var routine = await GetOwnedRoutineAsync(userId, routineId, ct);
		var summary = await _workoutRepository.GetRoutineSummaryAsync(userId, routineId, ct);
		var dates = await _workoutRepository.GetWorkoutDatesAsync(userId, routineId, ct);

		var overview = BuildOverview(routine.Id, routine.Name.Value, summary);
		return new RoutineStatsDto(
			overview.RoutineId,
			overview.Name,
			overview.WorkoutCount,
			overview.FirstWorkoutDate,
			overview.LastWorkoutDate,
			overview.DaysSinceLastWorkout,
			overview.WeeksInUse,
			overview.PrCount,
			BuildWeekActivity(dates));
	}

	public async Task<RoutineUsageDto> GetRoutineUsageAsync(
		string userId, Guid routineId, CancellationToken ct = default)
	{
		var routine = await GetOwnedRoutineAsync(userId, routineId, ct);
		var dates = await _workoutRepository.GetWorkoutDatesAsync(userId, routineId, ct);

		return new RoutineUsageDto(routine.Id, routine.Name.Value, BuildPeriods(dates));
	}

	public async Task<IReadOnlyList<ExerciseUsageDto>> GetTopExercisesAsync(
		string userId, Guid routineId, int limit = 10, CancellationToken ct = default)
	{
		await GetOwnedRoutineAsync(userId, routineId, ct);

		var frequency = await _workoutRepository.GetExerciseFrequencyAsync(userId, routineId, limit, ct);
		return frequency.Select(f => new ExerciseUsageDto(f.Name, f.SessionCount)).ToList();
	}

	private async Task<Routine> GetOwnedRoutineAsync(
		string userId, Guid routineId, CancellationToken ct)
		=> await _routineRepository.GetByIdAsync(userId, routineId, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

	private static RoutineOverviewDto BuildOverview(
		Guid routineId, string name, RoutineWorkoutSummary? summary)
	{
		if (summary is null || summary.WorkoutCount == 0)
			return new RoutineOverviewDto(routineId, name, 0, null, null, -1, 0, 0);

		var today = DateTime.UtcNow.Date;
		var first = summary.FirstDate!.Value.Date;
		var last = summary.LastDate!.Value.Date;
		var daysSinceLast = (today - last).Days;
		var weeksInUse = Math.Max(1, (today - first).Days / 7 + 1);

		return new RoutineOverviewDto(
			routineId, name, summary.WorkoutCount,
			summary.FirstDate, summary.LastDate, daysSinceLast, weeksInUse, summary.PrCount);
	}

	private static IReadOnlyList<WeekActivityDto> BuildWeekActivity(IReadOnlyList<DateTime> dates)
	{
		var counts = dates
			.GroupBy(StartOfWeek)
			.ToDictionary(g => g.Key, g => g.Count());

		var currentWeek = StartOfWeek(DateTime.UtcNow);
		var weeks = new List<WeekActivityDto>(ConsistencyWeeks);

		for (var i = ConsistencyWeeks - 1; i >= 0; i--)
		{
			var start = currentWeek.AddDays(-7 * i);
			weeks.Add(new WeekActivityDto(start, counts.GetValueOrDefault(start)));
		}

		return weeks;
	}

	private static IReadOnlyList<UsagePeriodDto> BuildPeriods(IReadOnlyList<DateTime> dates)
	{
		var ordered = dates.Select(d => d.Date).Distinct().OrderBy(d => d).ToList();
		if (ordered.Count == 0)
			return Array.Empty<UsagePeriodDto>();

		var periods = new List<UsagePeriodDto>();
		var start = ordered[0];
		var previous = ordered[0];
		var count = 1;

		for (var i = 1; i < ordered.Count; i++)
		{
			if ((ordered[i] - previous).TotalDays > UsageGapDays)
			{
				periods.Add(new UsagePeriodDto(start, previous, count));
				start = ordered[i];
				count = 0;
			}

			previous = ordered[i];
			count++;
		}

		// Último periodo abierto (End null = hasta el último uso / en curso)
		periods.Add(new UsagePeriodDto(start, null, count));
		return periods;
	}

	private static DateTime StartOfWeek(DateTime date)
	{
		var day = date.Date;
		var offset = ((int)day.DayOfWeek + 6) % 7; // Lunes = 0
		return day.AddDays(-offset);
	}
}
