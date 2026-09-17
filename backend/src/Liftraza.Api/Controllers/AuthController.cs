using AppValidationException = Liftraza.Application.Exceptions.ValidationException;
using Liftraza.Application.Abstractions;
using Liftraza.Application.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Liftraza.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly IValidator<RegisterRequest> _registerValidator;
	private readonly IValidator<LoginRequest> _loginValidator;
	private readonly IValidator<ForgotPasswordRequest> _forgotPasswordValidator;
	private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;

	public AuthController(
		IAuthService authService,
		IValidator<RegisterRequest> registerValidator,
		IValidator<LoginRequest> loginValidator,
		IValidator<ForgotPasswordRequest> forgotPasswordValidator,
		IValidator<ResetPasswordRequest> resetPasswordValidator)
	{
		_authService = authService;
		_registerValidator = registerValidator;
		_loginValidator = loginValidator;
		_forgotPasswordValidator = forgotPasswordValidator;
		_resetPasswordValidator = resetPasswordValidator;
	}

	[HttpPost("register")]
	[ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
	{
		var validation = await _registerValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors);

		return Ok(await _authService.RegisterAsync(request, ct));
	}

	[HttpPost("login")]
	[ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
	{
		var validation = await _loginValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors);

		return Ok(await _authService.LoginAsync(request, ct));
	}

	[HttpPost("forgot-password")]
	[ProducesResponseType(StatusCodes.Status202Accepted)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request, CancellationToken ct)
	{
		var validation = await _forgotPasswordValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors);

		await _authService.RequestPasswordResetAsync(request, ct);

		// Always accepted, so the endpoint never reveals which emails have an account.
		return Accepted();
	}

	[HttpPost("reset-password")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken ct)
	{
		var validation = await _resetPasswordValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors);

		await _authService.ResetPasswordAsync(request, ct);
		return NoContent();
	}
}
