namespace Liftraza.Application.Dtos;

public record RoutineDto(Guid Id, string Name, DateTime CreatedAt, DateTime? LastWorkoutDate);
