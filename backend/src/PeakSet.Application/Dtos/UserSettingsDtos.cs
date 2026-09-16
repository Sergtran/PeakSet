using PeakSet.Domain.Enums;

namespace PeakSet.Application.Dtos;

public record UserSettingsDto(
	Theme Theme,
	int TimerPrepSeconds,
	int TimerWorkSeconds,
	int TimerRestSeconds,
	int TimerSets,
	Guid? CurrentRoutineId);

public record UpdateUserSettingsRequest(
	Theme Theme,
	int TimerPrepSeconds,
	int TimerWorkSeconds,
	int TimerRestSeconds,
	int TimerSets);
