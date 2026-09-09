using PeakSet.Domain.Entities;
using PeakSet.Domain.Enums;
using PeakSet.Domain.Exceptions;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Tests;

public class WorkoutTests
{
	private const string UserId = "user-1";

	[Fact]
	public void Create_ShouldStoreSnapshotNamesAndDate()
	{
		var date = new DateTime(2026, 8, 3, 10, 30, 0, DateTimeKind.Utc);

		var workout = new Workout(UserId, new Name("Push / Pull"), new Name("Día 1"), date);

		Assert.Equal(UserId, workout.UserId);
		Assert.Equal("Push / Pull", workout.RoutineName.Value);
		Assert.Equal("Día 1", workout.SessionName.Value);
		Assert.Equal(date, workout.WorkoutDate);
		Assert.Null(workout.RoutineId);
	}

	[Fact]
	public void Create_WithEmptySnapshotNames_ShouldThrow()
	{
		var date = DateTime.UtcNow;

		Assert.Throws<ArgumentException>(() => new Workout(UserId, new Name(""), new Name("Día 1"), date));
		Assert.Throws<ArgumentException>(() => new Workout(UserId, new Name("Push"), new Name("  "), date));
	}

	[Fact]
	public void AddExercise_ShouldAssignOrderAndWorkoutId()
	{
		var workout = new Workout(UserId, new Name("Push"), new Name("Día 1"), DateTime.UtcNow);

		workout.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);
		var exercise = workout.AddExercise(new Name("Aperturas"), ExerciseType.Weighted, Laterality.Unilateral);

		Assert.Equal(2, workout.Exercises.Count);
		Assert.Equal(1, exercise.DisplayOrder);
		Assert.Equal(workout.Id, exercise.WorkoutId);
	}

	[Fact]
	public void AddSet_ShouldNumberSetsSequentially()
	{
		var workout = new Workout(UserId, new Name("Push"), new Name("Día 1"), DateTime.UtcNow);
		var exercise = workout.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);

		var first = exercise.AddSet(new Repetitions(10), new Weight(60m));
		var second = exercise.AddSet(new Repetitions(8), new Weight(70m));

		Assert.Equal(1, first.SetNumber);
		Assert.Equal(2, second.SetNumber);
		Assert.Equal(exercise.Id, first.WorkoutExerciseId);
		Assert.Equal(10, first.Reps.Value);
		Assert.Equal(60m, first.Weight.Value);
	}

	[Fact]
	public void AddSet_WithNegativeWeightOrReps_ShouldThrow()
	{
		var workout = new Workout(UserId, new Name("Push"), new Name("Día 1"), DateTime.UtcNow);
		var exercise = workout.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);

		// Las invariantes viven en los Value Objects: reps/peso negativos se rechazan ahí.
		Assert.Throws<ArgumentOutOfRangeException>(() => exercise.AddSet(new Repetitions(-1), new Weight(60m)));
		Assert.Throws<ArgumentOutOfRangeException>(() => exercise.AddSet(new Repetitions(10), new Weight(-5m)));
	}

	[Fact]
	public void RemoveSet_ShouldRenumberRemaining()
	{
		var workout = new Workout(UserId, new Name("Push"), new Name("Día 1"), DateTime.UtcNow);
		var exercise = workout.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);
		exercise.AddSet(new Repetitions(10), new Weight(60m));
		exercise.AddSet(new Repetitions(8), new Weight(70m));
		exercise.AddSet(new Repetitions(6), new Weight(80m));

		exercise.RemoveSet(2);

		Assert.Equal(2, exercise.Sets.Count);
		Assert.Equal(new[] { 1, 2 }, exercise.Sets.Select(s => s.SetNumber).ToArray());
		Assert.Equal(80m, exercise.Sets.Last().Weight.Value);
	}

	[Fact]
	public void RemoveSet_NotFound_ShouldThrowDomainException()
	{
		var workout = new Workout(UserId, new Name("Push"), new Name("Día 1"), DateTime.UtcNow);
		var exercise = workout.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);

		Assert.Throws<DomainException>(() => exercise.RemoveSet(99));
	}

	[Fact]
	public void SetPrStatus_ShouldStorePrState()
	{
		var workout = new Workout(UserId, new Name("Push"), new Name("Día 1"), DateTime.UtcNow);
		var exercise = workout.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);

		exercise.SetPrStatus(PrStatus.New);

		Assert.Equal(PrStatus.New, exercise.PrStatus);

		exercise.SetPrStatus(null);

		Assert.Null(exercise.PrStatus);
	}

}
