using PeakSet.Application.Dtos;

namespace PeakSet.Application.Abstractions;

public interface IRoutineService
{
	Task<RoutineDto> CreateRoutineAsync(string userId, CreateRoutineRequest request, CancellationToken ct = default);
	Task<IReadOnlyList<RoutineDto>> GetRoutinesAsync(string userId, CancellationToken ct = default);
	Task<RoutineDto> GetRoutineAsync(string userId, Guid id, CancellationToken ct = default);
	Task<SessionDto> AddSessionAsync(string userId, Guid routineId, CreateSessionRequest request, CancellationToken ct = default);
	Task<SessionExerciseDto> AddExerciseAsync(string userId, Guid routineId, Guid sessionId, CreateSessionExerciseRequest request, CancellationToken ct = default);
	Task<RoutineDetailDto> GetRoutineDetailAsync(string userId, Guid id, CancellationToken ct = default);
	Task<RoutineDto> RenameRoutineAsync(string userId, Guid id, CreateRoutineRequest request, CancellationToken ct = default);
	Task<SessionDto> RenameSessionAsync(string userId, Guid routineId, Guid sessionId, CreateSessionRequest request, CancellationToken ct = default);
	Task<SessionExerciseDto> EditExerciseAsync(string userId, Guid routineId, Guid sessionId, Guid exerciseId, CreateSessionExerciseRequest request, CancellationToken ct = default);
	Task DeleteSessionAsync(string userId, Guid routineId, Guid sessionId, CancellationToken ct = default);
	Task DeleteExerciseAsync(string userId, Guid routineId, Guid sessionId, Guid exerciseId, CancellationToken ct = default);
	Task DeleteRoutineAsync(string userId, Guid id, CancellationToken ct = default);
}
