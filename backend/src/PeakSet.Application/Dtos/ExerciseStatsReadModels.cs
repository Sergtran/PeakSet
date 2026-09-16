using PeakSet.Domain.Enums;
namespace PeakSet.Application.Dtos;

public record ExerciseSummary(
	string Name,
	int SessionCount,
	DateTime LastUsed);

public record ExerciseSessionRow(
	DateTime WorkoutDate,
	ExerciseType ExerciseType,
	Laterality Laterality,
	PrStatus? PrStatus,
	IReadOnlyList<(int Reps, decimal Weight)> Sets);
