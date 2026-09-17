using Liftraza.Domain.Enums;

namespace Liftraza.Application.Dtos;

public record ExerciseCatalogDto(
	string Name,
	ExerciseType ExerciseType,
	Laterality DefaultLaterality);
