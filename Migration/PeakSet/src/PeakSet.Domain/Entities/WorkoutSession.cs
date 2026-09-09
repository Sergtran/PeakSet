using PeakSet.Domain.Common;
using PeakSet.Domain.Enums;
using PeakSet.Domain.Exceptions;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Domain.Entities;

/// <summary>
/// Sesión dentro de una rutina (ej.: "Día 1 - Push").
/// Puede crearse vacía y completarse después (regla de negocio 4.2).
/// </summary>
public sealed class WorkoutSession : Entity
{
	public const int MaxNameLength = 100;

	private readonly List<SessionExercise> _exercises = new();

	private WorkoutSession()
	{
		// Requerido por EF Core
	}

	public WorkoutSession(Name name)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Session name cannot exceed {MaxNameLength} characters.", nameof(name));

		Id = Guid.NewGuid();
		Name = name;
	}

	public Guid RoutineId { get; private set; }

	public Name Name { get; private set; } = null!;

	public int DisplayOrder { get; private set; }

	public IReadOnlyCollection<SessionExercise> Exercises => _exercises.AsReadOnly();

	#region Behaviors

	public void Rename(Name name)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Session name cannot exceed {MaxNameLength} characters.", nameof(name));

		Name = name;
	}

	public SessionExercise AddExercise(Name name, ExerciseType exerciseType, Laterality laterality)
	{
		var displayOrder = _exercises.Count > 0 ? _exercises.Max(e => e.DisplayOrder) + 1 : 0;

		var exercise = new SessionExercise(name, exerciseType, laterality);
		exercise.SetWorkoutSessionId(Id);
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

	public void MoveExercise(Guid exerciseId, int newPosition)
	{
		var exercise = FindExercise(exerciseId);

		_exercises.Remove(exercise);
		newPosition = Math.Clamp(newPosition, 0, _exercises.Count);
		_exercises.Insert(newPosition, exercise);
		RenumberExercises();
	}

	/// <summary>
	/// Copia profunda: nueva identidad, mismos datos y orden de ejercicios.
	/// (El orden lo reasigna el agregado raíz al agregar la copia a la rutina.)
	/// </summary>
	public WorkoutSession Clone()
	{
		var clone = new WorkoutSession(Name);

		foreach (var exercise in _exercises)
			clone.AddExercise(exercise.Name, exercise.ExerciseType, exercise.Laterality);

		return clone;
	}

	#endregion

	private SessionExercise FindExercise(Guid exerciseId)
	{
		return _exercises.FirstOrDefault(e => e.Id == exerciseId)
			?? throw new DomainException("Exercise not found in session.");
	}

	private void RenumberExercises()
	{
		// Invariante: el orden de la lista es el orden canónico (lista[i].DisplayOrder == i).
		for (var i = 0; i < _exercises.Count; i++)
			_exercises[i].Renumber(i);
	}

	internal void SetRoutineId(Guid routineId)
	{
		RoutineId = routineId;
	}

	internal void Renumber(int displayOrder)
	{
		DisplayOrder = displayOrder;
	}
}
