using GymTracker.Application.Dtos;

namespace GymTracker.Application.Abstractions;

public interface IBackupService
{
	Task<GymTrackerExportDto> ExportAsync(string userId, CancellationToken ct = default);
	Task ImportAsync(string userId, GymTrackerExportDto snapshot, CancellationToken ct = default);
}