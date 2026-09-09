using System.Security.Claims;
using PeakSet.Application.Abstractions;
using PeakSet.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PeakSet.Api.Controllers;

[ApiController]
[Route("api/home")]
[Authorize]
public class HomeController : ControllerBase
{
	private readonly IRoutineStatsService _statsService;

	public HomeController(IRoutineStatsService statsService)
		=> _statsService = statsService;

	private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
		?? throw new InvalidOperationException("User id claim not found.");

	[HttpGet]
	[ProducesResponseType(typeof(HomeDto), StatusCodes.Status200OK)]
	public async Task<ActionResult<HomeDto>> Get(CancellationToken ct)
		=> Ok(await _statsService.GetHomeAsync(UserId, ct));
}
