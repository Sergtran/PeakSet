using PeakSet.Domain.Enums;

namespace PeakSet.Application.Dtos;

public record WorkoutSetDto(int SetNumber, int Reps, decimal Weight);

public record WorkoutExerciseDto(
	Guid Id, string Name, ExerciseType ExerciseType,
	Laterality Laterality, PrStatus? PrStatus,
	IReadOnlyList<WorkoutSetDto> Sets);

public record WorkoutDto(
	Guid Id, Guid? RoutineId, string RoutineName, string SessionName,
	DateTime WorkoutDate, DateTime CreatedAt,
	IReadOnlyList<WorkoutExerciseDto> Exercises);