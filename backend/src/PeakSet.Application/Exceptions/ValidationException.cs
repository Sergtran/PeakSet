namespace PeakSet.Application.Exceptions;

public sealed class ValidationException : Exception
{
	public IReadOnlyCollection<string> Errors { get; }

	public ValidationException(IEnumerable<string> errors)
		: base("The request is not valid.")
	{
		Errors = errors.ToList().AsReadOnly();
	}
}