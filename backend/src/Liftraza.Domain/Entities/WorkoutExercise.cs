using Liftraza.Domain.Common;
using Liftraza.Domain.Enums;
using Liftraza.Domain.Exceptions;
using Liftraza.Domain.ValueObjects;

namespace Liftraza.Domain.Entities;

/// <summary>
/// Exercise recorded inside a completed workout (history).
/// Stores a snapshot of name/type/laterality and the PR status computed on save (ADR-006).
/// </summary>
public sealed class WorkoutExercise : Entity
{
	public const int MaxNameLength = 150;

	private readonly List<WorkoutSet> _sets = new();

	private WorkoutExercise()
	{
		// Requerido por EF Core
	}

	public WorkoutExercise(Name name, ExerciseType exerciseType, Laterality laterality)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Exercise name cannot exceed {MaxNameLength} characters.", nameof(name));

		Id = Guid.NewGuid();
		Name = name;
		ExerciseType = exerciseType;
		Laterality = laterality;
	}

	public Guid WorkoutId { get; private set; }

	public Name Name { get; private set; } = null!;

	public ExerciseType ExerciseType { get; private set; }

	public Laterality Laterality { get; private set; }

	/// <summary>
	/// PR computed when the workout is saved. <see langword="null"/> = no PR.
	/// </summary>
	public PrStatus? PrStatus { get; private set; }

	public int DisplayOrder { get; private set; }

	public IReadOnlyCollection<WorkoutSet> Sets => _sets.AsReadOnly();

	#region Behaviors

	public WorkoutSet AddSet(Repetitions reps, Weight weight)
	{
		var setNumber = _sets.Count > 0 ? _sets.Max(s => s.SetNumber) + 1 : 1;

		var set = new WorkoutSet(setNumber, reps, weight);
		set.SetWorkoutExerciseId(Id);
		_sets.Add(set);

		return set;
	}

	public void RemoveSet(int setNumber)
	{
		var set = _sets.FirstOrDefault(s => s.SetNumber == setNumber)
			?? throw new DomainException($"Set {setNumber} not found in exercise '{Name.Value}'.");

		_sets.Remove(set);
		RenumberSets();
	}

	public void SetPrStatus(PrStatus? prStatus)
	{
		PrStatus = prStatus;
	}

	#endregion

	private void RenumberSets()
	{
		// Invariant: list order is the canonical order (list[i].SetNumber == i + 1).
		for (var i = 0; i < _sets.Count; i++)
			_sets[i].Renumber(i + 1);
	}

	internal void SetWorkoutId(Guid workoutId)
	{
		WorkoutId = workoutId;
	}

	internal void Renumber(int displayOrder)
	{
		DisplayOrder = displayOrder;
	}
}
