namespace PeakSet.Application.Dtos;

// Read models: resultados de queries agregadas sobre Workouts (Fase 5 — Active Routine + Stats)
public record RoutineWorkoutSummary(int WorkoutCount, DateTime? FirstDate, DateTime? LastDate, int PrCount);

public record ExerciseFrequency(string Name, int SessionCount);
