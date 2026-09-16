using PeakSet.Domain.Enums;

namespace PeakSet.Application.Dtos;

public record ProgressPointDto(
	DateTime Date,
	decimal? Weight,
	int? Reps,
	PrStatus? PrStatus);

public record ExerciseProgressDto(
	string Name,
	int SessionCount,
	int TotalSets,
	decimal? BestWeight,
	int? BestReps,
	DateTime? FirstSeen,
	DateTime? LastSeen,
	IReadOnlyList<ProgressPointDto> Timeline);

public record ExerciseSummaryDto(
	string Name,
	int SessionCount,
	DateTime? LastUsed);	