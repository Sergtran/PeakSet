using GymTracker.Application.Dtos;

namespace GymTracker.Application.Abstractions;

public interface ICalendarService
{
	Task<CalendarMonthDto> GetMonthAsync(string userId, int year, int month, CancellationToken ct = default);
}
