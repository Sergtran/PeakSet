using PeakSet.Application.Dtos;

namespace PeakSet.Application.Abstractions;

public interface IAuthService
{
	Task<AuthResponse> RegisterAsync(
		RegisterRequest request,
		CancellationToken cancellationToken = default);

	Task<AuthResponse> LoginAsync(
		LoginRequest request,
		CancellationToken cancellationToken = default);

	Task RequestPasswordResetAsync(
		ForgotPasswordRequest request,
		CancellationToken cancellationToken = default);

	Task ResetPasswordAsync(
		ResetPasswordRequest request,
		CancellationToken cancellationToken = default);
}
