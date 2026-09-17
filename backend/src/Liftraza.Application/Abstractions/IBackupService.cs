using Liftraza.Application.Dtos;

namespace Liftraza.Application.Abstractions;

public interface IBackupService
{
	Task<LiftrazaExportDto> ExportAsync(string userId, CancellationToken ct = default);
	Task ImportAsync(string userId, LiftrazaExportDto snapshot, CancellationToken ct = default);
}