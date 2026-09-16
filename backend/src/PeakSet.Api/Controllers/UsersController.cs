using System.Security.Claims;
using PeakSet.Application.Abstractions;
using PeakSet.Application.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppValidationException = PeakSet.Application.Exceptions.ValidationException;

namespace PeakSet.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
	private readonly ICurrentRoutineService _currentRoutineService;
	private readonly IUserSettingsService _settingsService;
	private readonly IValidator<UpdateUserSettingsRequest> _settingsValidator;

	public UsersController(
		ICurrentRoutineService currentRoutineService,
		IUserSettingsService settingsService,
		IValidator<UpdateUserSettingsRequest> settingsValidator)
	{
		_currentRoutineService = currentRoutineService;
		_settingsService = settingsService;
		_settingsValidator = settingsValidator;
	}

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

	[HttpGet("me/settings")]
	[ProducesResponseType(typeof(UserSettingsDto), StatusCodes.Status200OK)]
	public async Task<ActionResult<UserSettingsDto>> GetSettings(CancellationToken ct)
		=> Ok(await _settingsService.GetAsync(UserId, ct));

	[HttpPut("me/settings")]
	[ProducesResponseType(typeof(UserSettingsDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<UserSettingsDto>> UpdateSettings(
		UpdateUserSettingsRequest request, CancellationToken ct)
	{
		var validation = await _settingsValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors);

		return Ok(await _settingsService.UpdateAsync(UserId, request, ct));
	}
}
