namespace PeakSet.Domain.Exceptions;

/// <summary>
/// Excepción base para violaciones de reglas de negocio del dominio.
/// Usar <see cref="ArgumentException"/> para validar argumentos y
/// esta excepción para reglas de negocio (ej.: no se puede avanzar una semana inexistente).
/// </summary>
public class DomainException : Exception
{
	public DomainException(string message)
		: base(message)
	{
	}

	public DomainException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
