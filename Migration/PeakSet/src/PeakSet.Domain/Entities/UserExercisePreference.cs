using PeakSet.Domain.Common;
using PeakSet.Domain.Enums;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Domain.Entities;

/// <summary>
/// Preferencia del usuario sobre un ejercicio del catálogo: override de tipo/lateralidad
/// (index.html: exerciseTypes / exerciseLaterality por usuario).
/// Unicidad por (UserId, ExerciseName) garantizada con índice único en BD.
/// </summary>
public sealed class UserExercisePreference : Entity
{
	private UserExercisePreference()
	{
		// Requerido por EF Core
	}

	public UserExercisePreference(string userId, Name exerciseName, ExerciseType exerciseType, Laterality laterality)
	{
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("UserId cannot be empty.", nameof(userId));
		ArgumentNullException.ThrowIfNull(exerciseName);

		Id = Guid.NewGuid();
		UserId = userId;
		ExerciseName = exerciseName;
		ExerciseType = exerciseType;
		Laterality = laterality;
	}

	public string UserId { get; private set; } = string.Empty;

	public Name ExerciseName { get; private set; } = null!;

	public ExerciseType ExerciseType { get; private set; }

	public Laterality Laterality { get; private set; }

	#region Behaviors

	public void Update(ExerciseType exerciseType, Laterality laterality)
	{
		ExerciseType = exerciseType;
		Laterality = laterality;
	}

	#endregion
}
