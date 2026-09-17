using PeakSet.Domain.Enums;

namespace PeakSet.Application.Dtos;

public record ExerciseCatalogDto(
	string Name,
	ExerciseType ExerciseType,
	Laterality DefaultLaterality);
