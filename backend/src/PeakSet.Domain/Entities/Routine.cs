using PeakSet.Domain.Common;
using PeakSet.Domain.Exceptions;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Domain.Entities;

/// <summary>
/// Training routine (plan). Root of the planning aggregate.
/// Rules: the name is required (max 100 characters) and every routine belongs to a user
/// (UserId comes from the token, never from the request body — see documentation, section 4.2).
/// Names are modeled with the <see cref="Name"/> value object: the domain never receives
/// a raw string when an abstraction exists that encapsulates its rules.
/// </summary>
public sealed class Routine : Entity
{
	public const int MaxNameLength = 100;

	private readonly List<WorkoutSession> _sessions = new();

	private Routine()
	{
		// Requerido por EF Core
	}

	public Routine(string userId, Name name)
	{
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("UserId cannot be empty.", nameof(userId));

		Id = Guid.NewGuid();
		UserId = userId;
		Rename(name);

		CreatedAt = DateTime.UtcNow;
		UpdatedAt = CreatedAt;
	}

	public string UserId { get; private set; } = string.Empty;

	public Name Name { get; private set; } = null!;

	public DateTime CreatedAt { get; private set; }

	public DateTime UpdatedAt { get; private set; }

	public IReadOnlyCollection<WorkoutSession> Sessions => _sessions.AsReadOnly();

	#region Behaviors

	public void Rename(Name name)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Routine name cannot exceed {MaxNameLength} characters.", nameof(name));

		Name = name;

		Touch();
	}

	public void AddSession(WorkoutSession session)
	{
		ArgumentNullException.ThrowIfNull(session);

		var displayOrder = _sessions.Count > 0 ? _sessions.Max(s => s.DisplayOrder) + 1 : 0;

		session.SetRoutineId(Id);
		session.Renumber(displayOrder);
		_sessions.Add(session);

		Touch();
	}

	public void RemoveSession(Guid sessionId)
	{
		var session = FindSession(sessionId);

		_sessions.Remove(session);
		RenumberSessions();

		Touch();
	}

	public void MoveSession(Guid sessionId, int newPosition)
	{
		var session = FindSession(sessionId);

		_sessions.Remove(session);
		newPosition = Math.Clamp(newPosition, 0, _sessions.Count);
		_sessions.Insert(newPosition, session);
		RenumberSessions();

		Touch();
	}

	/// <summary>
	/// Clones a session (new Id, same content) and appends it to the end of the routine.
	/// </summary>
	public WorkoutSession DuplicateSession(Guid sessionId)
	{
		var session = FindSession(sessionId);

		var copy = session.Clone();
		AddSession(copy);

		return copy;
	}

	#endregion

	private WorkoutSession FindSession(Guid sessionId)
	{
		return _sessions.FirstOrDefault(x => x.Id == sessionId)
			?? throw new DomainException("Session not found in routine.");
	}

	private void RenumberSessions()
	{
		// Invariant: list order is the canonical order (list[i].DisplayOrder == i).
		for (var i = 0; i < _sessions.Count; i++)
			_sessions[i].Renumber(i);
	}

	private void Touch()
	{
		UpdatedAt = DateTime.UtcNow;
	}
}
