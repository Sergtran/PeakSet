using Liftraza.Domain.Enums;

namespace Liftraza.Application.Dtos;

public record SessionExerciseDto(
	Guid Id, string Name, ExerciseType ExerciseType,
	Laterality Laterality, int DisplayOrder);