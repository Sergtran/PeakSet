namespace PeakSet.Application.Abstractions;

public interface ICurrentRoutineService
{
    Task SetCurrentRoutineAsync(string userId, Guid? routineId, CancellationToken ct = default);
}
