namespace PeakSet.Application.Dtos;

public record AuthResponse(
	string Token,
	string Email,
	string? DisplayName);