using PeakSet.Application.Abstractions;
using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Application.Dtos;
using PeakSet.Application.Exceptions;

namespace PeakSet.Application.Services;

public sealed class CalendarService : ICalendarService
{
	private readonly IWorkoutRepository _workoutRepository;

	public CalendarService(IWorkoutRepository workoutRepository)
		=> _workoutRepository = workoutRepository;

	public async Task<CalendarMonthDto> GetMonthAsync(
		string userId, int year, int month, CancellationToken ct = default)
	{
		if (year is < 2000 or > 2100)
			throw new ValidationException(new[] { "El año debe estar entre 2000 y 2100." });
		if (month is < 1 or > 12)
			throw new ValidationException(new[] { "El mes debe estar entre 1 y 12." });

		// Npgsql exige DateTime con Kind=Utc para comparar contra timestamp with time zone.
		var from = DateTime.SpecifyKind(new DateTime(year, month, 1), DateTimeKind.Utc);
		var to = from.AddMonths(1);

		var rows = await _workoutRepository.GetCalendarWorkoutsAsync(userId, from, to, ct);

		var days = rows
			.GroupBy(r => r.WorkoutDate.Date)
			.OrderBy(g => g.Key)
			.Select(g => new CalendarDayDto(
				g.Key,
				g.Select(r => new CalendarWorkoutDto(r.Id, r.WorkoutDate, r.RoutineName, r.SessionName)).ToList()))
			.ToList();

		return new CalendarMonthDto(year, month, days);
	}
}
