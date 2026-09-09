using PeakSet.Application.Abstractions;
using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Infrastructure.Persistence.Repositories;
using PeakSet.Infrastructure.Security;
using PeakSet.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PeakSet.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
		services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IRoutineRepository, EfRoutineRepository>();
		services.AddScoped<IWorkoutRepository, EfWorkoutRepository>();
		services.AddScoped<IUserSettingsRepository, EfUserSettingsRepository>();
		return services;
	}
}
