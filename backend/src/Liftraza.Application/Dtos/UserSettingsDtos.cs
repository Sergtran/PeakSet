using Liftraza.Domain.Enums;

namespace Liftraza.Application.Dtos;

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
