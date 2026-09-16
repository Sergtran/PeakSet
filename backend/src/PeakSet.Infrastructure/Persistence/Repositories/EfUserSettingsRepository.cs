using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Domain.Entities;
using PeakSet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PeakSet.Infrastructure.Persistence.Repositories;

public sealed class EfUserSettingsRepository : IUserSettingsRepository
{
	private readonly PeakSetDbContext _db;

	public EfUserSettingsRepository(PeakSetDbContext db)
		=> _db = db;

	public async Task<UserSettings?> GetByUserIdAsync(string userId, CancellationToken ct = default)
		=> await _db.UserSettings
			.AsTracking()
			.FirstOrDefaultAsync(s => s.UserId == userId, ct);

	public async Task AddAsync(UserSettings settings, CancellationToken ct = default)
	{
		_db.UserSettings.Add(settings);
		await _db.SaveChangesAsync(ct);
	}

	public async Task UpdateAsync(UserSettings settings, CancellationToken ct = default)
		=> await _db.SaveChangesAsync(ct);
}
