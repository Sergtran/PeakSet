using PeakSet.Application.Dtos;

namespace PeakSet.Application.Abstractions;

public interface IRoutineStatsService
{
    Task<HomeDto> GetHomeAsync(string userId, CancellationToken ct = default);
    Task<RoutineStatsDto> GetRoutineStatsAsync(string userId, Guid routineId, CancellationToken ct = default);
    Task<RoutineUsageDto> GetRoutineUsageAsync(string userId, Guid routineId, CancellationToken ct = default);
    Task<IReadOnlyList<ExerciseUsageDto>> GetTopExercisesAsync(
        string userId, Guid routineId, int limit = 10, CancellationToken ct = default);
}
