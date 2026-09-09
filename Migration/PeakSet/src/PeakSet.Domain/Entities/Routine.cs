using PeakSet.Domain.Common;
using PeakSet.Domain.Exceptions;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Domain.Entities;

/// <summary>
/// Rutina de entrenamiento (plan). Raíz del agregado de planificación.
/// Reglas: el nombre es obligatorio (máx. 100 caracteres) y toda rutina pertenece a un usuario
/// (UserId viene del token, nunca del request body — ver documentación, sección 4.2).
/// Los nombres se modelan con el Value Object <see cref="Name"/>: el dominio nunca recibe
/// un string crudo cuando existe una abstracción que encapsula sus reglas.
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
	/// Clona una sesión (nuevo Id, mismo contenido) y la agrega al final de la rutina.
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
		// Invariante: el orden de la lista es el orden canónico (lista[i].DisplayOrder == i).
		for (var i = 0; i < _sessions.Count; i++)
			_sessions[i].Renumber(i);
	}

	private void Touch()
	{
		UpdatedAt = DateTime.UtcNow;
	}
}
