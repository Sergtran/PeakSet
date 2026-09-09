using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Domain.Entities;
using PeakSet.Domain.ValueObjects;
using PeakSet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PeakSet.Infrastructure.Persistence.Repositories;

public sealed class EfRoutineRepository : IRoutineRepository
{
	private readonly PeakSetDbContext _db;

	public EfRoutineRepository(PeakSetDbContext db)
		=> _db = db;

	public async Task<Routine?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default)
		=> await _db.Routines
			.AsTracking()
			.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == id, ct);

	public async Task<IReadOnlyList<Routine>> GetByUserAsync(string userId, CancellationToken ct = default)
		=> await _db.Routines
			.AsNoTracking()
			.Where(r => r.UserId == userId)
			.OrderBy(r => r.CreatedAt)
			.ToListAsync(ct);

	public async Task<bool> ExistsByNameAsync(string userId, string name, Guid? excludeId = null, CancellationToken ct = default)
		=> await _db.Routines
			.AnyAsync(r => r.UserId == userId
				&& r.Name == new Name(name)
				&& (!excludeId.HasValue || r.Id != excludeId.Value), ct);

	public async Task AddAsync(Routine routine, CancellationToken ct = default)
	{
		_db.Routines.Add(routine);
		await _db.SaveChangesAsync(ct);
	}

	public async Task<Routine?> GetByIdWithSessionsAsync(string userId, Guid id, CancellationToken ct = default)
	=> await _db.Routines
		.AsTracking()
		.AsSplitQuery()
		.Include(r => r.Sessions)
			.ThenInclude(s => s.Exercises)
		.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == id, ct);

	public async Task UpdateAsync(Routine routine, CancellationToken ct = default)
		=> await _db.SaveChangesAsync(ct);

	public async Task DeleteAsync(Routine routine, CancellationToken ct = default)
	{
		_db.Routines.Remove(routine);
		await _db.SaveChangesAsync(ct);
	}

	public async Task<IReadOnlyList<Routine>> GetByUserWithSessionsAsync(
	string userId, CancellationToken ct = default)
	=> await _db.Routines
		.AsNoTracking()
		.AsSplitQuery()                       
		.Where(r => r.UserId == userId)
		.OrderBy(r => r.CreatedAt)
		.Include(r => r.Sessions)
			.ThenInclude(s => s.Exercises)
		.ToListAsync(ct);
}
