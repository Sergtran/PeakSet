using Liftraza.Domain.Entities;

namespace Liftraza.Application.Abstractions.Repositories;

public interface IUserSettingsRepository
{
    Task<UserSettings?> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task AddAsync(UserSettings settings, CancellationToken ct = default);
    Task UpdateAsync(UserSettings settings, CancellationToken ct = default);
}
