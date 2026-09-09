using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PeakSet.Application.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace PeakSet.Infrastructure.Security;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
	private readonly JwtSettings _settings;

	public JwtTokenGenerator(IOptions<JwtSettings> settings)
		=> _settings = settings.Value;

	public string GenerateToken(string userId, string email, string displayName)
	{
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, userId),
			new Claim(JwtRegisteredClaimNames.Email, email),
			new Claim(JwtRegisteredClaimNames.Name, displayName)
		};

		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
			SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _settings.Issuer,
			audience: _settings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
			signingCredentials: signingCredentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}