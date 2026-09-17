namespace Liftraza.Domain.Exceptions;

/// <summary>
/// Base exception for domain business rule violations.
/// Usar <see cref="ArgumentException"/> para validar argumentos y
/// this exception for business rules (e.g. you cannot advance a week that does not exist).
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
