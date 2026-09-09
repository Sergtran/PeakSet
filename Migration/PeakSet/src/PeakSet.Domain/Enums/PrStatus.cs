namespace PeakSet.Domain.Enums;

/// <summary>
/// Estado de récord personal (PR) de un ejercicio al guardar un entrenamiento.
/// La ausencia de PR se representa con <see langword="null"/> (nullable PrStatus).
/// </summary>
public enum PrStatus
{
	New = 0,
	Matched = 1
}
