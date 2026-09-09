namespace PeakSet.Application.Dtos;

public record RoutineDetailDto(
	Guid Id, string Name, DateTime CreatedAt,
	IReadOnlyList<SessionDto> Sessions);