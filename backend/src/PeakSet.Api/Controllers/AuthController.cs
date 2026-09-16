using AppValidationException = PeakSet.Application.Exceptions.ValidationException;
using PeakSet.Application.Abstractions;
using PeakSet.Application.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace PeakSet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly IValidator<RegisterRequest> _registerValidator;
	private readonly IValidator<LoginRequest> _loginValidator;

	public AuthController(
		IAuthService authService,
		IValidator<RegisterRequest> registerValidator,
		IValidator<LoginRequest> loginValidator)
	{
		_authService = authService;
		_registerValidator = registerValidator;
		_loginValidator = loginValidator;
	}

	[HttpPost("register")]
	[ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
	{
		var validation = await _registerValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		return Ok(await _authService.RegisterAsync(request, ct));
	}

	[HttpPost("login")]
	[ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
	{
		var validation = await _loginValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		return Ok(await _authService.LoginAsync(request, ct));
	}
}