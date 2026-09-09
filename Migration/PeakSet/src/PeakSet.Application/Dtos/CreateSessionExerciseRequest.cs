using PeakSet.Domain.Enums;

namespace PeakSet.Application.Dtos;

public record CreateSessionExerciseRequest(
	string Name,
	ExerciseType ExerciseType,
	Laterality Laterality);