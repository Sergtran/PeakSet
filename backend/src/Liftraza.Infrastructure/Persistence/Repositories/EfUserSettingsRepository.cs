using Liftraza.Application.Abstractions.Repositories;
using Liftraza.Domain.Entities;
using Liftraza.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Liftraza.Infrastructure.Persistence.Repositories;

public sealed class EfUserSettingsRepository : IUserSettingsRepository
{
	private readonly LiftrazaDbContext _db;

	public EfUserSettingsRepository(LiftrazaDbContext db)
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
