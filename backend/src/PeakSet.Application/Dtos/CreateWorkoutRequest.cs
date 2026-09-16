using PeakSet.Domain.Enums;

namespace PeakSet.Application.Dtos;

public record CreateWorkoutSetRequest(int Reps, decimal Weight);

public record CreateWorkoutExerciseRequest(
	string Name,
	ExerciseType ExerciseType,
	Laterality Laterality,
	IReadOnlyList<CreateWorkoutSetRequest> Sets);

public record CreateWorkoutRequest(
	Guid? RoutineId,
	string RoutineName,
	string SessionName,
	DateTime WorkoutDate,
	IReadOnlyList<CreateWorkoutExerciseRequest> Exercises);