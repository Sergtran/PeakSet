using Liftraza.Domain.Common;
using Liftraza.Domain.ValueObjects;

namespace Liftraza.Domain.Entities;

/// <summary>
/// Set recorded in a workout exercise.
/// The reps (non-negative) and weight (non-negative) invariants live in the value objects
/// <see cref="Repetitions"/> and <see cref="Weight"/>; the entity validates the set number.
/// For time exercises, "reps" stores seconds.
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
