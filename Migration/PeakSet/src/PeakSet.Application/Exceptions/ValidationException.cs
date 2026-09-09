namespace PeakSet.Application.Exceptions;

public sealed class ValidationException : Exception
{
	public IReadOnlyCollection<string> Errors { get; }

	public ValidationException(IEnumerable<string> errors)
		: base("La solicitud no es válida.")
	{
		Errors = errors.ToList().AsReadOnly();
	}
}