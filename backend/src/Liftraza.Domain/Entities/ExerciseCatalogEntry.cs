using Liftraza.Domain.Common;
using Liftraza.Domain.Enums;
using Liftraza.Domain.ValueObjects;

namespace Liftraza.Domain.Entities;

/// <summary>
/// Global exercise catalog entry (seeded from the app default catalog).
/// No UserId: it is shared by all users. The name is UNIQUE in the database.
/// Per-user preferences are stored in <see cref="UserExercisePreference"/>.
/// Like every other aggregate it uses Guid; the seed IDs are deterministic GUIDs
/// (uuid5 over the name, generated once) so HasData stays stable.
/// </summary>
public sealed class ExerciseCatalogEntry : Entity
{
	public const int MaxNameLength = 150;

	private ExerciseCatalogEntry()
	{
		// Requerido por EF Core
	}

	public ExerciseCatalogEntry(Guid id, Name name, ExerciseType exerciseType, Laterality defaultLaterality)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Exercise name cannot exceed {MaxNameLength} characters.", nameof(name));

		Id = id;
		Name = name;
		ExerciseType = exerciseType;
		DefaultLaterality = defaultLaterality;
	}

	public ExerciseCatalogEntry(Name name, ExerciseType exerciseType, Laterality defaultLaterality)
		: this(Guid.NewGuid(), name, exerciseType, defaultLaterality)
	{
	}

	public Name Name { get; private set; } = null!;

	public ExerciseType ExerciseType { get; private set; }

	public Laterality DefaultLaterality { get; private set; }

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

	public void SetDefaultLaterality(Laterality laterality)
	{
		DefaultLaterality = laterality;
	}

	#endregion
}
