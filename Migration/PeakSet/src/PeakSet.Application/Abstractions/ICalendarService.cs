using PeakSet.Application.Dtos;

namespace PeakSet.Application.Abstractions;

public interface ICalendarService
{
	Task<CalendarMonthDto> GetMonthAsync(string userId, int year, int month, CancellationToken ct = default);
}
