using PeakSet.Application.Abstractions;
using PeakSet.Application.Dtos;
using PeakSet.Application.Exceptions;
using PeakSet.Domain.Entities;
using PeakSet.Infrastructure.Data;
using PeakSet.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace PeakSet.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IJwtTokenGenerator _tokenGenerator;
	private readonly PeakSetDbContext _db;

	public AuthService(
		UserManager<ApplicationUser> userManager,
		IJwtTokenGenerator tokenGenerator,
		PeakSetDbContext db)
	{
		_userManager = userManager;
		_tokenGenerator = tokenGenerator;
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
			throw new ValidationException(result.Errors.Select(e => e.Description));

		// Pendiente de la Fase 2: UserSettings por defecto al registrarse (doc, paso 8).
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
}