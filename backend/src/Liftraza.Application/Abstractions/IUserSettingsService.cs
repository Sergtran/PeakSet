using Liftraza.Application.Dtos;

namespace Liftraza.Application.Abstractions;

public interface IUserSettingsService
{
	Task<UserSettingsDto> GetAsync(string userId, CancellationToken ct = default);

	Task<UserSettingsDto> UpdateAsync(
		string userId, UpdateUserSettingsRequest request, CancellationToken ct = default);
}
