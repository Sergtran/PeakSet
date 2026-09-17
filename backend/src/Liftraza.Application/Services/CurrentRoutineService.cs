using Liftraza.Application.Abstractions;
using Liftraza.Application.Abstractions.Repositories;
using Liftraza.Application.Exceptions;
using Liftraza.Domain.Entities;

namespace Liftraza.Application.Services;

public sealed class CurrentRoutineService : ICurrentRoutineService
{
	private readonly IUserSettingsRepository _settingsRepository;
	private readonly IRoutineRepository _routineRepository;

	public CurrentRoutineService(
		IUserSettingsRepository settingsRepository,
		IRoutineRepository routineRepository)
	{
		_settingsRepository = settingsRepository;
		_routineRepository = routineRepository;
	}

	public async Task SetCurrentRoutineAsync(
		string userId, Guid? routineId, CancellationToken ct = default)
	{
		var settings = await _settingsRepository.GetByUserIdAsync(userId, ct);
		var created = settings is null;
		settings ??= new UserSettings(userId);

		if (routineId is null)
		{
			settings.ClearCurrentRoutine();
		}
		else
		{
			var routine = await _routineRepository.GetByIdAsync(userId, routineId.Value, ct)
				?? throw new NotFoundException("Routine not found.");

			settings.SetCurrentRoutine(routine.Id);
		}

		if (created)
			await _settingsRepository.AddAsync(settings, ct);
		else
			await _settingsRepository.UpdateAsync(settings, ct);
	}
}
