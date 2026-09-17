namespace Liftraza.Domain.Enums;

/// <summary>
/// Personal record (PR) status of an exercise when a workout is saved.
/// No PR is represented with <see langword="null"/> (nullable PrStatus).
/// </summary>
public enum PrStatus
{
	New = 0,
	Matched = 1
}
