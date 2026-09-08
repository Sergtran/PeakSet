using FluentValidation;
using GymTracker.Application.Abstractions;
using GymTracker.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymTracker.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
		services.AddScoped<IRoutineService, RoutineService>();
		services.AddScoped<IWorkoutService, WorkoutService>();
		services.AddScoped<ICurrentRoutineService, CurrentRoutineService>();
		services.AddScoped<IRoutineStatsService, RoutineStatsService>();
		services.AddScoped<IExerciseStatsService, ExerciseStatsService>();
		services.AddScoped<ICalendarService, CalendarService>();
		return services;
	}
}
