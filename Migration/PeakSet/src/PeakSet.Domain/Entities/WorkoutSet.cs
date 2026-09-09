using PeakSet.Domain.Common;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Domain.Entities;

/// <summary>
/// Serie registrada en un ejercicio de un entrenamiento.
/// Las invariantes de reps (no negativas) y peso (no negativo) viven en los Value Objects
/// <see cref="Repetitions"/> y <see cref="Weight"/>; la entidad valida el número de serie.
/// Para ejercicios de tiempo, "reps" almacena segundos — igual que index.html.
/// </summary>
public sealed class WorkoutSet : Entity
{
	private WorkoutSet()
	{
		// Requerido por EF Core
	}

	public WorkoutSet(int setNumber, Repetitions reps, Weight weight)
	{
		if (setNumber < 1)
			throw new ArgumentOutOfRangeException(nameof(setNumber), "Set number must be greater than zero.");
		ArgumentNullException.ThrowIfNull(reps);
		ArgumentNullException.ThrowIfNull(weight);

		Id = Guid.NewGuid();
		SetNumber = setNumber;
		Reps = reps;
		Weight = weight;
	}

	public Guid WorkoutExerciseId { get; private set; }

	public int SetNumber { get; private set; }

	public Repetitions Reps { get; private set; } = null!;

	public Weight Weight { get; private set; } = null!;

	internal void SetWorkoutExerciseId(Guid workoutExerciseId)
	{
		WorkoutExerciseId = workoutExerciseId;
	}

	internal void Renumber(int setNumber)
	{
		SetNumber = setNumber;
	}
}
