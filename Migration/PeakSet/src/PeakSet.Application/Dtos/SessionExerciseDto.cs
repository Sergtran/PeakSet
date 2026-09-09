using PeakSet.Domain.Enums;

namespace PeakSet.Application.Dtos;

public record SessionExerciseDto(
	Guid Id, string Name, ExerciseType ExerciseType,
	Laterality Laterality, int DisplayOrder);