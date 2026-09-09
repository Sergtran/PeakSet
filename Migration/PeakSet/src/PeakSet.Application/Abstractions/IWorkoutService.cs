using PeakSet.Application.Dtos;

namespace PeakSet.Application.Abstractions;

public interface IWorkoutService
{
	Task<WorkoutDto> CreateWorkoutAsync(string userId, CreateWorkoutRequest request, CancellationToken ct = default);
	Task<PagedResult<WorkoutDto>> GetWorkoutsAsync(string userId, int page, int pageSize, CancellationToken ct = default);
	Task<WorkoutDto> GetWorkoutAsync(string userId, Guid id, CancellationToken ct = default);
	Task<WorkoutDto> EditWorkoutAsync(string userId, Guid id, CreateWorkoutRequest request, CancellationToken ct = default);
	Task DeleteWorkoutAsync(string userId, Guid id, CancellationToken ct = default);
}
