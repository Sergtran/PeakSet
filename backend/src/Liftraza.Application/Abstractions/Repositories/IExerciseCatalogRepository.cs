using Liftraza.Domain.Entities;

namespace Liftraza.Application.Abstractions.Repositories;

public interface IExerciseCatalogRepository
{
	Task<IReadOnlyList<ExerciseCatalogEntry>> GetAllAsync(CancellationToken ct = default);
}
