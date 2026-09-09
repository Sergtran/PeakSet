namespace PeakSet.Application.Dtos;

public record RoutineOverviewDto(
    Guid RoutineId,
    string Name,
    int WorkoutCount,
    DateTime? FirstWorkoutDate,
    DateTime? LastWorkoutDate,
    int DaysSinceLastWorkout,
    int WeeksInUse,
    int PrCount);
