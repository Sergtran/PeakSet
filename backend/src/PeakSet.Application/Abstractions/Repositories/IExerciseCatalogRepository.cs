using PeakSet.Domain.Entities;

namespace PeakSet.Application.Abstractions.Repositories;

public interface IExerciseCatalogRepository
{
	Task<IReadOnlyList<ExerciseCatalogEntry>> GetAllAsync(CancellationToken ct = default);
}
