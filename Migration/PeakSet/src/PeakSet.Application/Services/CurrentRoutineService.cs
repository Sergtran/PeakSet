using PeakSet.Application.Abstractions;
using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Application.Exceptions;
using PeakSet.Domain.Entities;

namespace PeakSet.Application.Services;

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
				?? throw new NotFoundException("Rutina no encontrada.");

			settings.SetCurrentRoutine(routine.Id);
		}

		if (created)
			await _settingsRepository.AddAsync(settings, ct);
		else
			await _settingsRepository.UpdateAsync(settings, ct);
	}
}
