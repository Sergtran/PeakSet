using FluentValidation;
using Liftraza.Application.Abstractions;
using Liftraza.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Liftraza.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
		services.AddScoped<IRoutineService, RoutineService>();
		services.AddScoped<IWorkoutService, WorkoutService>();
		services.AddScoped<ICurrentRoutineService, CurrentRoutineService>();
		services.AddScoped<IUserSettingsService, UserSettingsService>();
		services.AddScoped<IRoutineStatsService, RoutineStatsService>();
		services.AddScoped<IExerciseStatsService, ExerciseStatsService>();
		services.AddScoped<ICalendarService, CalendarService>();
		return services;
	}
}
