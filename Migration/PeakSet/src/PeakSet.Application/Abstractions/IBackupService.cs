using PeakSet.Application.Dtos;

namespace PeakSet.Application.Abstractions;

public interface IBackupService
{
	Task<PeakSetExportDto> ExportAsync(string userId, CancellationToken ct = default);
	Task ImportAsync(string userId, PeakSetExportDto snapshot, CancellationToken ct = default);
}