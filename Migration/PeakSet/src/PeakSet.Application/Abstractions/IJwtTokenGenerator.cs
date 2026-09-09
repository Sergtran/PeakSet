namespace PeakSet.Application.Abstractions;

public interface IJwtTokenGenerator
{
	string GenerateToken(
		string userId,
		string email,
		string displayName);
}	