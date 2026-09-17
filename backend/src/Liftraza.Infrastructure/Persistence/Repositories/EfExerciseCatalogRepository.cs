using Liftraza.Application.Abstractions.Repositories;
using Liftraza.Domain.Entities;
using Liftraza.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Liftraza.Infrastructure.Persistence.Repositories;

public sealed class EfExerciseCatalogRepository : IExerciseCatalogRepository
{
	private readonly LiftrazaDbContext _db;

	public EfExerciseCatalogRepository(LiftrazaDbContext db)
		=> _db = db;

	public async Task<IReadOnlyList<ExerciseCatalogEntry>> GetAllAsync(CancellationToken ct = default)
	{
		var entries = await _db.ExerciseCatalogEntries
			.AsNoTracking()
			.ToListAsync(ct);

		return entries.OrderBy(entry => entry.Name.Value, StringComparer.OrdinalIgnoreCase).ToList();
	}
}
