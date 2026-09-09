using PeakSet.Domain.Common;
using PeakSet.Domain.Enums;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Domain.Entities;

/// <summary>
/// Ejercicio dentro de una sesión planificada.
/// Guarda snapshot de nombre/tipo/lateralidad (ADR-004): el plan no depende del catálogo
/// y sobrevive aunque el ejercicio se renombre o elimine del catálogo.
/// </summary>
public sealed class SessionExercise : Entity
{
	public const int MaxNameLength = 150;

	private SessionExercise()
	{
		// Requerido por EF Core
	}

	public SessionExercise(Name name, ExerciseType exerciseType, Laterality laterality)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Exercise name cannot exceed {MaxNameLength} characters.", nameof(name));

		Id = Guid.NewGuid();
		Name = name;
		ExerciseType = exerciseType;
		Laterality = laterality;
	}

	public Guid WorkoutSessionId { get; private set; }

	public Name Name { get; private set; } = null!;

	public ExerciseType ExerciseType { get; private set; }

	public Laterality Laterality { get; private set; }

	public int DisplayOrder { get; private set; }

	#region Behaviors

	public void Rename(Name name)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Exercise name cannot exceed {MaxNameLength} characters.", nameof(name));

		Name = name;
	}

	public void SetExerciseType(ExerciseType exerciseType)
	{
		ExerciseType = exerciseType;
	}

	public void SetLaterality(Laterality laterality)
	{
		Laterality = laterality;
	}

	#endregion

	internal void SetWorkoutSessionId(Guid workoutSessionId)
	{
		WorkoutSessionId = workoutSessionId;
	}

	internal void Renumber(int displayOrder)
	{
		DisplayOrder = displayOrder;
	}
}
