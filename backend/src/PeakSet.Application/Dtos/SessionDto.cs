namespace PeakSet.Application.Dtos;

public record SessionDto(
	Guid Id, string Name, int DisplayOrder,
	IReadOnlyList<SessionExerciseDto> Exercises);