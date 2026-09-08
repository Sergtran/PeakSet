using GymTracker.Domain.Common;
using GymTracker.Domain.Enums;
using GymTracker.Domain.Exceptions;
using GymTracker.Domain.ValueObjects;

namespace GymTracker.Domain.Entities;

/// <summary>
/// Entrenamiento realizado (historial).
/// Guarda RoutineName/SessionName como snapshot (ADR-004/ADR-005): el historial sigue siendo
/// legible aunque la rutina se borre o renombre. RoutineId es nullable con FK RESTRICT:
/// borrar una rutina no borra el historial.
/// </summary>
public sealed class Workout : Entity
{
	private readonly List<WorkoutExercise> _exercises = new();

	private Workout()
	{
		// Requerido por EF Core
	}

	public Workout(string userId, Name routineName, Name sessionName, DateTime workoutDate, Guid? routineId = null)
	{
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("UserId cannot be empty.", nameof(userId));
		ArgumentNullException.ThrowIfNull(routineName);
		ArgumentNullException.ThrowIfNull(sessionName);

		Id = Guid.NewGuid();
		UserId = userId;
		RoutineId = routineId;
		RoutineName = routineName;
		SessionName = sessionName;
		WorkoutDate = workoutDate;
		CreatedAt = DateTime.UtcNow;
	}

	public string UserId { get; private set; } = string.Empty;

	public Guid? RoutineId { get; private set; }

	public Name RoutineName { get; private set; } = null!;

	public Name SessionName { get; private set; } = null!;

	public DateTime WorkoutDate { get; private set; }

	public DateTime CreatedAt { get; private set; }

	public IReadOnlyCollection<WorkoutExercise> Exercises => _exercises.AsReadOnly();

	#region Behaviors

	public WorkoutExercise AddExercise(Name name, ExerciseType exerciseType, Laterality laterality)
	{
		var displayOrder = _exercises.Count > 0 ? _exercises.Max(e => e.DisplayOrder) + 1 : 0;

		var exercise = new WorkoutExercise(name, exerciseType, laterality);
		exercise.SetWorkoutId(Id);
		exercise.Renumber(displayOrder);
		_exercises.Add(exercise);

		return exercise;
	}

	public void RemoveExercise(Guid exerciseId)
	{
		var exercise = FindExercise(exerciseId);

		_exercises.Remove(exercise);
		RenumberExercises();
	}

	#endregion

	private WorkoutExercise FindExercise(Guid exerciseId)
	{
		return _exercises.FirstOrDefault(e => e.Id == exerciseId)
			?? throw new DomainException("Exercise not found in workout.");
	}

	private void RenumberExercises()
	{
		// Invariante: el orden de la lista es el orden canónico (lista[i].DisplayOrder == i).
		for (var i = 0; i < _exercises.Count; i++)
			_exercises[i].Renumber(i);
	}
}
