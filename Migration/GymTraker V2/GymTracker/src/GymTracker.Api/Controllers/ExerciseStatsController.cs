using System.Security.Claims;
using GymTracker.Application.Abstractions;
using GymTracker.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.Api.Controllers;

[ApiController]
[Route("api/exercises")]
[Authorize]
public class ExerciseStatsController : ControllerBase
{
	private readonly IExerciseStatsService _statsService;

	public ExerciseStatsController(IExerciseStatsService statsService)
		=> _statsService = statsService;

	private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
		?? throw new InvalidOperationException("User id claim not found.");

	[HttpGet]
	[ProducesResponseType(typeof(IReadOnlyList<ExerciseSummaryDto>), StatusCodes.Status200OK)]
	public async Task<ActionResult<IReadOnlyList<ExerciseSummaryDto>>> GetExercises(CancellationToken ct)
		=> Ok(await _statsService.GetExercisesAsync(UserId, ct));

	[HttpGet("{name}/progress")]
	[ProducesResponseType(typeof(ExerciseProgressDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<ExerciseProgressDto>> GetProgress(string name, CancellationToken ct)
	{
		var progress = await _statsService.GetProgressAsync(UserId, name, ct);
		return progress is null ? NotFound() : Ok(progress);
	}
}
