namespace PeakSet.Application.Dtos;

public record UsagePeriodDto(DateTime Start, DateTime? End, int WorkoutCount);

public record RoutineUsageDto(Guid RoutineId, string Name, IReadOnlyList<UsagePeriodDto> Periods);
