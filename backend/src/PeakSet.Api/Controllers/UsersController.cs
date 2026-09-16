using System.Security.Claims;
using PeakSet.Application.Abstractions;
using PeakSet.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PeakSet.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
	private readonly ICurrentRoutineService _currentRoutineService;

	public UsersController(ICurrentRoutineService currentRoutineService)
		=> _currentRoutineService = currentRoutineService;

	private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
		?? throw new InvalidOperationException("User id claim not found.");

	[HttpPut("me/current-routine")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> SetCurrentRoutine(SetCurrentRoutineRequest request, CancellationToken ct)
	{
		await _currentRoutineService.SetCurrentRoutineAsync(UserId, request.RoutineId, ct);
		return NoContent();
	}
}
