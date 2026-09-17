using Liftraza.Domain.Common;
using Liftraza.Domain.ValueObjects;

namespace Liftraza.Domain.Entities;

/// <summary>
/// Personal note about an exercise (e.g. technique, pain, progress).
/// Uniqueness on (UserId, ExerciseName) is enforced by a unique index in the database
/// (surrogate Id key + UNIQUE index; see documentation, section 8, ADR-010).
/// </summary>
public sealed class ExerciseNote : Entity
{
	private ExerciseNote()
	{
		// Requerido por EF Core
	}

	public ExerciseNote(string userId, Name exerciseName, string text, DateTime noteDate)
	{
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("UserId cannot be empty.", nameof(userId));
		ArgumentNullException.ThrowIfNull(exerciseName);
		if (string.IsNullOrWhiteSpace(text))
			throw new ArgumentException("Note text cannot be empty.", nameof(text));

		Id = Guid.NewGuid();
		UserId = userId;
		ExerciseName = exerciseName;
		Text = text.Trim();
		NoteDate = noteDate;
	}

	public string UserId { get; private set; } = string.Empty;

	public Name ExerciseName { get; private set; } = null!;

	public string Text { get; private set; } = string.Empty;

	public DateTime NoteDate { get; private set; }

	#region Behaviors

	public void UpdateText(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
			throw new ArgumentException("Note text cannot be empty.", nameof(text));

		Text = text.Trim();
	}

	#endregion
}
