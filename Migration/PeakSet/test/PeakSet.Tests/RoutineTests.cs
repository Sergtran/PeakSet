using PeakSet.Domain.Entities;
using PeakSet.Domain.Enums;
using PeakSet.Domain.Exceptions;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Tests;

public class RoutineTests
{
	private const string UserId = "user-1";

	[Fact]
	public void Create_ShouldSetUserIdNameAndEmptySessions()
	{
		var routine = new Routine(UserId, new Name("Push / Pull"));

		Assert.Equal(UserId, routine.UserId);
		Assert.Equal("Push / Pull", routine.Name.Value);
		Assert.Empty(routine.Sessions);
		Assert.NotEqual(Guid.Empty, routine.Id);
	}

	[Fact]
	public void Create_WithEmptyName_ShouldThrow()
	{
		// El Value Object Name rechaza el nombre vacío antes de llegar a la entidad.
		Assert.Throws<ArgumentException>(() => new Name("  "));
		Assert.Throws<ArgumentException>(() => new Routine(UserId, new Name("")));
	}

	[Fact]
	public void Create_WithEmptyUserId_ShouldThrow()
	{
		Assert.Throws<ArgumentException>(() => new Routine("  ", new Name("Push")));
	}

	[Fact]
	public void Rename_ShouldUpdateName()
	{
		var routine = new Routine(UserId, new Name("Push"));

		routine.Rename(new Name("Full Body"));

		Assert.Equal("Full Body", routine.Name.Value);
		Assert.True(routine.UpdatedAt >= routine.CreatedAt);
	}

	[Fact]
	public void Rename_WithTooLongName_ShouldThrow()
	{
		var routine = new Routine(UserId, new Name("Push"));
		var tooLong = new string('a', Routine.MaxNameLength + 1);

		Assert.Throws<ArgumentException>(() => routine.Rename(new Name(tooLong)));
	}

	[Fact]
	public void AddSession_ShouldAssignRoutineIdAndDisplayOrder()
	{
		var routine = new Routine(UserId, new Name("Push"));
		var session = new WorkoutSession(new Name("Día 1"));

		routine.AddSession(session);

		var added = Assert.Single(routine.Sessions);
		Assert.Equal(routine.Id, added.RoutineId);
		Assert.Equal(0, added.DisplayOrder);
	}

	[Fact]
	public void AddSessions_ShouldIncreaseDisplayOrder()
	{
		var routine = new Routine(UserId, new Name("Push"));
		routine.AddSession(new WorkoutSession(new Name("Día 1")));
		routine.AddSession(new WorkoutSession(new Name("Día 2")));
		routine.AddSession(new WorkoutSession(new Name("Día 3")));

		Assert.Equal(new[] { 0, 1, 2 }, routine.Sessions.Select(s => s.DisplayOrder).ToArray());
	}

	[Fact]
	public void MoveSession_ShouldReorderAndRenumber()
	{
		var routine = new Routine(UserId, new Name("Push"));
		var a = new WorkoutSession(new Name("A"));
		var b = new WorkoutSession(new Name("B"));
		var c = new WorkoutSession(new Name("C"));
		routine.AddSession(a);
		routine.AddSession(b);
		routine.AddSession(c);

		routine.MoveSession(c.Id, 0);

		Assert.Equal(new[] { "C", "A", "B" }, routine.Sessions.Select(s => s.Name.Value).ToArray());
		Assert.Equal(new[] { 0, 1, 2 }, routine.Sessions.Select(s => s.DisplayOrder).ToArray());
	}

	[Fact]
	public void RemoveSession_ShouldRenumberRemaining()
	{
		var routine = new Routine(UserId, new Name("Push"));
		routine.AddSession(new WorkoutSession(new Name("A")));
		var b = new WorkoutSession(new Name("B"));
		routine.AddSession(b);
		routine.AddSession(new WorkoutSession(new Name("C")));

		routine.RemoveSession(b.Id);

		Assert.Equal(2, routine.Sessions.Count);
		Assert.Equal(new[] { 0, 1 }, routine.Sessions.Select(s => s.DisplayOrder).ToArray());
	}

	[Fact]
	public void RemoveSession_NotFound_ShouldThrowDomainException()
	{
		var routine = new Routine(UserId, new Name("Push"));

		Assert.Throws<DomainException>(() => routine.RemoveSession(Guid.NewGuid()));
	}

	[Fact]
	public void DuplicateSession_ShouldAppendIndependentCopyWithExercises()
	{
		var routine = new Routine(UserId, new Name("Push"));
		var session = new WorkoutSession(new Name("Día 1"));
		session.AddExercise(new Name("Press Banca"), ExerciseType.Weighted, Laterality.Bilateral);
		routine.AddSession(session);

		var copy = routine.DuplicateSession(session.Id);

		Assert.Equal(2, routine.Sessions.Count);
		Assert.NotEqual(session.Id, copy.Id);
		Assert.Equal("Día 1", copy.Name.Value);
		Assert.Equal(1, copy.DisplayOrder); // segunda sesión de la rutina
		Assert.Single(copy.Exercises);
		Assert.Equal("Press Banca", copy.Exercises.First().Name.Value);
	}
}
