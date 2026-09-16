using PeakSet.Domain.Enums;

namespace PeakSet.Application.Dtos;

public record PeakSetExportDto(
	int Version,                       
	DateTime ExportedAtUtc,
	UserSettingsExportDto Settings,
	IReadOnlyList<RoutineExportDto> Routines,
	IReadOnlyList<WorkoutExportDto> Workouts);

public record UserSettingsExportDto(
	Theme Theme,
	int TimerPrepSeconds,
	int TimerWorkSeconds,
	int TimerRestSeconds,
	int TimerSets,
	Guid? CurrentRoutineId);

public record RoutineExportDto(
	Guid Id,
	string Name,
	IReadOnlyList<SessionExportDto> Sessions);

public record SessionExportDto(
	Guid Id,
	string Name,
	int DisplayOrder,
	IReadOnlyList<SessionExerciseExportDto> Exercises);

public record SessionExerciseExportDto(
	Guid Id,
	string Name,
	ExerciseType ExerciseType,
	Laterality Laterality,
	int DisplayOrder);

public record WorkoutExportDto(
	Guid Id,
	Guid? RoutineId,
	string RoutineName,
	string SessionName,
	DateTime WorkoutDate,
	IReadOnlyList<WorkoutExerciseExportDto> Exercises);

public record WorkoutExerciseExportDto(
	Guid Id,
	string Name,
	ExerciseType ExerciseType,
	Laterality Laterality,
	PrStatus? PrStatus,
	int DisplayOrder,
	IReadOnlyList<WorkoutSetExportDto> Sets);

public record WorkoutSetExportDto(
	Guid Id,
	int SetNumber,
	int Reps,
	decimal Weight);