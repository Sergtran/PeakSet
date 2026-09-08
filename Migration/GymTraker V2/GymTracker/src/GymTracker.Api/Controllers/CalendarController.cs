using System.Security.Claims;
using GymTracker.Application.Abstractions;
using GymTracker.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.Api.Controllers;

[ApiController]
[Route("api/calendar")]
[Authorize]
public class CalendarController : ControllerBase
{
	private readonly ICalendarService _calendarService;

	public CalendarController(ICalendarService calendarService)
		=> _calendarService = calendarService;

	private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
		?? throw new InvalidOperationException("User id claim not found.");

	[HttpGet("{year:int}/{month:int}")]
	[ProducesResponseType(typeof(CalendarMonthDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CalendarMonthDto>> GetMonth(int year, int month, CancellationToken ct)
		=> Ok(await _calendarService.GetMonthAsync(UserId, year, month, ct));
}
