using Liftraza.Application.Dtos;

namespace Liftraza.Application.Abstractions;

public interface ICalendarService
{
	Task<CalendarMonthDto> GetMonthAsync(string userId, int year, int month, CancellationToken ct = default);
}
