namespace PeakSet.Application.Dtos;

public record CalendarWorkoutRow(Guid Id, DateTime WorkoutDate, string RoutineName, string SessionName);

public record CalendarWorkoutDto(Guid Id, DateTime Date, string RoutineName, string SessionName);

public record CalendarDayDto(DateTime Date, IReadOnlyList<CalendarWorkoutDto> Workouts);

public record CalendarMonthDto(int Year, int Month, IReadOnlyList<CalendarDayDto> Days);
