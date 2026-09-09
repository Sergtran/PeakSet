using PeakSet.Domain.Common;
using PeakSet.Domain.Enums;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Domain.Entities;

/// <summary>
/// Ejercicio del catálogo global (seed a partir de DEFAULT_EXERCISES de index.html).
/// Sin UserId: es compartido por todos los usuarios. El nombre es UNIQUE en BD.
/// Las preferencias individuales se guardan en <see cref="UserExercisePreference"/>.
/// Igual que los demás agregados, usa Guid; los IDs del seed son GUIDs deterministas
/// (uuid5 sobre el nombre, generados una sola vez) para que HasData sea estable.
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
