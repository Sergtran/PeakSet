using PeakSet.Domain.Entities;

namespace PeakSet.Application.Abstractions.Repositories;

public interface IRoutineRepository
{
	Task<Routine?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default);
	Task<IReadOnlyList<Routine>> GetByUserAsync(string userId, CancellationToken ct = default);
	Task<bool> ExistsByNameAsync(string userId, string name, Guid? excludeId = null, CancellationToken ct = default);
	Task AddAsync(Routine routine, CancellationToken ct = default);
	Task<Routine?> GetByIdWithSessionsAsync(string userId, Guid id, CancellationToken ct = default);
	Task UpdateAsync(Routine routine, CancellationToken ct = default);
	Task DeleteAsync(Routine routine, CancellationToken ct = default);
	Task<IReadOnlyList<Routine>> GetByUserWithSessionsAsync(string userId, CancellationToken ct = default);
}
