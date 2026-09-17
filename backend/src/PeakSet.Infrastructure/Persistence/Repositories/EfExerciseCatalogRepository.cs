using PeakSet.Application.Abstractions.Repositories;
using PeakSet.Domain.Entities;
using PeakSet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PeakSet.Infrastructure.Persistence.Repositories;

public sealed class EfExerciseCatalogRepository : IExerciseCatalogRepository
{
	private readonly PeakSetDbContext _db;

	public EfExerciseCatalogRepository(PeakSetDbContext db)
		=> _db = db;

	public async Task<IReadOnlyList<ExerciseCatalogEntry>> GetAllAsync(CancellationToken ct = default)
	{
		var entries = await _db.ExerciseCatalogEntries
			.AsNoTracking()
			.ToListAsync(ct);

		return entries.OrderBy(entry => entry.Name.Value, StringComparer.OrdinalIgnoreCase).ToList();
	}
}
