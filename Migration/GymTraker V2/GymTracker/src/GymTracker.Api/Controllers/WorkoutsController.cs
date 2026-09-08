using System.Security.Claims;
using GymTracker.Application.Abstractions;
using GymTracker.Application.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppValidationException = GymTracker.Application.Exceptions.ValidationException;

namespace GymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkoutsController : ControllerBase
{
	private readonly IWorkoutService _workoutService;
	private readonly IValidator<CreateWorkoutRequest> _createValidator;

	public WorkoutsController(IWorkoutService workoutService, IValidator<CreateWorkoutRequest> createValidator)
	{
		_workoutService = workoutService;
		_createValidator = createValidator;
	}

	private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
		?? throw new InvalidOperationException("User id claim not found.");

	[HttpPost]
	[ProducesResponseType(typeof(WorkoutDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<WorkoutDto>> Create(CreateWorkoutRequest request, CancellationToken ct)
	{
		var validation = await _createValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		var workout = await _workoutService.CreateWorkoutAsync(UserId, request, ct);
		return CreatedAtAction(nameof(GetById), new { id = workout.Id }, workout);
	}

	[HttpGet]
	[ProducesResponseType(typeof(PagedResult<WorkoutDto>), StatusCodes.Status200OK)]
	public async Task<ActionResult<PagedResult<WorkoutDto>>> GetAll(
		[FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
		=> Ok(await _workoutService.GetWorkoutsAsync(UserId, page, pageSize, ct));

	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(WorkoutDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<WorkoutDto>> GetById(Guid id, CancellationToken ct)
		=> Ok(await _workoutService.GetWorkoutAsync(UserId, id, ct));

	[HttpPut("{id:guid}")]
	[ProducesResponseType(typeof(WorkoutDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<WorkoutDto>> Edit(Guid id, CreateWorkoutRequest request, CancellationToken ct)
	{
		var validation = await _createValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		return Ok(await _workoutService.EditWorkoutAsync(UserId, id, request, ct));
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
	{
		await _workoutService.DeleteWorkoutAsync(UserId, id, ct);
		return NoContent();
	}
}
