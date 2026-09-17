using Liftraza.Application.Abstractions;
using Liftraza.Application.Abstractions.Repositories;
using Liftraza.Application.Dtos;
using Liftraza.Domain.Entities;

namespace Liftraza.Application.Services;

public sealed class UserSettingsService : IUserSettingsService
{
	private readonly IUserSettingsRepository _repository;

	public UserSettingsService(IUserSettingsRepository repository)
		=> _repository = repository;

	public async Task<UserSettingsDto> GetAsync(string userId, CancellationToken ct = default)
	{
		var settings = await _repository.GetByUserIdAsync(userId, ct);

		if (settings is null)
		{
			settings = new UserSettings(userId);
			await _repository.AddAsync(settings, ct);
		}

		return Map(settings);
	}

	public async Task<UserSettingsDto> UpdateAsync(
		string userId, UpdateUserSettingsRequest request, CancellationToken ct = default)
	{
		var settings = await _repository.GetByUserIdAsync(userId, ct);
		var isNew = settings is null;
		settings ??= new UserSettings(userId);

		settings.SetTheme(request.Theme);
		settings.UpdateTimer(
			request.TimerPrepSeconds,
			request.TimerWorkSeconds,
			request.TimerRestSeconds,
			request.TimerSets);

		if (isNew)
			await _repository.AddAsync(settings, ct);
		else
			await _repository.UpdateAsync(settings, ct);

		return Map(settings);
	}

	private static UserSettingsDto Map(UserSettings settings)
		=> new(
			settings.Theme,
			settings.TimerPrepSeconds,
			settings.TimerWorkSeconds,
			settings.TimerRestSeconds,
			settings.TimerSets,
			settings.CurrentRoutineId);
}
