using Liftraza.Application.Abstractions;
using Liftraza.Application.Dtos;
using Liftraza.Application.Exceptions;
using Liftraza.Domain.Entities;
using Liftraza.Infrastructure.Data;
using Liftraza.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Liftraza.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IJwtTokenGenerator _tokenGenerator;
	private readonly IEmailSender _emailSender;
	private readonly LiftrazaDbContext _db;

	public AuthService(
		UserManager<ApplicationUser> userManager,
		IJwtTokenGenerator tokenGenerator,
		IEmailSender emailSender,
		LiftrazaDbContext db)
	{
		_userManager = userManager;
		_tokenGenerator = tokenGenerator;
		_emailSender = emailSender;
		_db = db;
	}

	public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
	{
		var user = new ApplicationUser
		{
			UserName = request.Email,
			Email = request.Email,
			DisplayName = request.DisplayName
		};

		var result = await _userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded)
			throw new ValidationException(
				result.Errors.Select(e => new ValidationError(e.Code, e.Description)));

		// Create default UserSettings on registration.
		_db.UserSettings.Add(new UserSettings(user.Id));
		await _db.SaveChangesAsync(ct);

		return new AuthResponse(
			_tokenGenerator.GenerateToken(user.Id, user.Email!, user.DisplayName ?? string.Empty),
			user.Email!,
			user.DisplayName);
	}

	public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);
		if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
			throw new InvalidCredentialsException();

		return new AuthResponse(
			_tokenGenerator.GenerateToken(user.Id, user.Email!, user.DisplayName ?? string.Empty),
			user.Email!,
			user.DisplayName);
	}

	public async Task RequestPasswordResetAsync(
		ForgotPasswordRequest request, CancellationToken ct = default)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		// Accounts are not enumerated: an unknown email produces the same response.
		if (user is null || user.Email is null)
			return;

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		var body =
			$"""
			Liftraza password reset

			Paste this token in the app to choose a new password:

			{token}

			If you did not ask for this, ignore this message.
			""";

		await _emailSender.SendAsync(user.Email, "Liftraza password reset", body, ct);
	}

	public async Task ResetPasswordAsync(
		ResetPasswordRequest request, CancellationToken ct = default)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		if (user is null)
			throw new ValidationException(new[]
			{
				new ValidationError("InvalidResetToken", "The reset token is not valid or has expired.")
			});

		var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
		if (!result.Succeeded)
			throw new ValidationException(
				result.Errors.Select(e => new ValidationError(e.Code, e.Description)));
	}
}
