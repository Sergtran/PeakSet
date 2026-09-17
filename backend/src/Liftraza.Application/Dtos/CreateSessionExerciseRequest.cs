using Liftraza.Domain.Enums;

namespace Liftraza.Application.Dtos;

public record CreateSessionExerciseRequest(
	string Name,
	ExerciseType ExerciseType,
	Laterality Laterality);