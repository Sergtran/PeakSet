namespace PeakSet.Application.Dtos;

public record WeekActivityDto(DateTime WeekStart, int WorkoutCount);

public record RoutineStatsDto(
    Guid RoutineId,
    string Name,
    int WorkoutCount,
    DateTime? FirstWorkoutDate,
    DateTime? LastWorkoutDate,
    int DaysSinceLastWorkout,
    int WeeksInUse,
    int PrCount,
    IReadOnlyList<WeekActivityDto> LastWeeks);
