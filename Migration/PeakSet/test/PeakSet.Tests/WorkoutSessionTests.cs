using PeakSet.Domain.Entities;
using PeakSet.Domain.Enums;
using PeakSet.Domain.Exceptions;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Tests;

public class WorkoutSessionTests
{
	[Fact]
	public void AddExercise_ShouldAssignDisplayOrderAndSessionId()
	{
		var session = new WorkoutSession(new Name("Día 1"));

		session.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);
		var exercise = session.AddExercise(new Name("Dominadas"), ExerciseType.Bodyweight, Laterality.Bilateral);

		Assert.Equal(2, session.Exercises.Count);
		Assert.Equal(1, exercise.DisplayOrder);
		Assert.Equal(session.Id, exercise.WorkoutSessionId);
	}

	[Fact]
	public void AddExercise_WithEmptyName_ShouldThrow()
	{
		var session = new WorkoutSession(new Name("Día 1"));

		Assert.Throws<ArgumentException>(() => session.AddExercise(new Name("  "), ExerciseType.Standard, Laterality.Bilateral));
	}

	[Fact]
	public void AddExercise_WithTooLongName_ShouldThrow()
	{
		var session = new WorkoutSession(new Name("Día 1"));
		var tooLong = new string('a', SessionExercise.MaxNameLength + 1);

		Assert.Throws<ArgumentException>(() => session.AddExercise(new Name(tooLong), ExerciseType.Standard, Laterality.Bilateral));
	}

	[Fact]
	public void MoveExercise_ShouldReorderAndRenumber()
	{
		var session = new WorkoutSession(new Name("Día 1"));
		var a = session.AddExercise(new Name("Press"), ExerciseType.Weighted, Laterality.Bilateral);
		var b = session.AddExercise(new Name("Remo"), ExerciseType.Weighted, Laterality.Bilateral);
		var c = session.AddExercise(new Name("Curl"), ExerciseType.Weighted, Laterality.Bilateral);

		session.MoveExercise(c.Id, 0);

		Assert.Equal(new[] { "Curl", "Press", "Remo" }, session.Exercises.Select(e => e.Name.Value).ToArray());
		Assert.Equal(new[] { 0, 1, 2 }, session.Exercises.Select(e => e.DisplayOrder).ToArray());
		Assert.Equal(a.Id, session.Exercises.Skip(1).First().Id);
	}

	[Fact]
	public void RemoveExercise_ShouldRenumberRemaining()
	{
		var session = new WorkoutSession(new Name("Día 1"));
		var a = session.AddExercise(new Name("Press"), ExerciseType.Weighted, Laterality.Bilateral);
		session.AddExercise(new Name("Remo"), ExerciseType.Weighted, Laterality.Bilateral);

		session.RemoveExercise(a.Id);

		var remaining = Assert.Single(session.Exercises);
		Assert.Equal("Remo", remaining.Name.Value);
		Assert.Equal(0, remaining.DisplayOrder);
	}

	[Fact]
	public void RemoveExercise_NotFound_ShouldThrowDomainException()
	{
		var session = new WorkoutSession(new Name("Día 1"));

		Assert.Throws<DomainException>(() => session.RemoveExercise(Guid.NewGuid()));
	}

	[Fact]
	public void Clone_ShouldCreateIndependentCopyWithSameExercises()
	{
		var session = new WorkoutSession(new Name("Día 1"));
		session.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);
		session.AddExercise(new Name("Aperturas"), ExerciseType.Weighted, Laterality.Unilateral);

		var clone = session.Clone();

		Assert.NotEqual(session.Id, clone.Id);
		Assert.Equal(session.Name.Value, clone.Name.Value);
		Assert.Equal(session.Exercises.Select(e => e.Name), clone.Exercises.Select(e => e.Name));
		Assert.Equal(session.Exercises.Select(e => e.ExerciseType), clone.Exercises.Select(e => e.ExerciseType));
		Assert.Equal(session.Exercises.Select(e => e.Laterality), clone.Exercises.Select(e => e.Laterality));
		Assert.All(clone.Exercises, e => Assert.NotEqual(session.Exercises.First(x => x.Name == e.Name).Id, e.Id));
	}

	[Fact]
	public void Rename_WithEmptyName_ShouldThrow()
	{
		var session = new WorkoutSession(new Name("Día 1"));

		Assert.Throws<ArgumentException>(() => session.Rename(new Name("")));
	}
}
