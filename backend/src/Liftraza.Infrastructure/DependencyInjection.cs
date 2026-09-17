using Liftraza.Application.Abstractions;
using Liftraza.Application.Abstractions.Repositories;
using Liftraza.Infrastructure.Persistence.Repositories;
using Liftraza.Infrastructure.Security;
using Liftraza.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liftraza.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
		services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IEmailSender, LoggingEmailSender>();
		services.AddScoped<IBackupService, BackupService>();
		services.AddScoped<IRoutineRepository, EfRoutineRepository>();
		services.AddScoped<IExerciseCatalogRepository, EfExerciseCatalogRepository>();
		services.AddScoped<IWorkoutRepository, EfWorkoutRepository>();
		services.AddScoped<IUserSettingsRepository, EfUserSettingsRepository>();
		return services;
	}
}
