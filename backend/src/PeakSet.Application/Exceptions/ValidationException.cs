using FluentValidation.Results;

namespace PeakSet.Application.Exceptions;

public sealed class ValidationException : Exception
{
	public IReadOnlyCollection<ValidationError> Errors { get; }

	public ValidationException(IEnumerable<ValidationError> errors)
		: this(errors.ToList().AsReadOnly())
	{
	}

	public ValidationException(IEnumerable<ValidationFailure> failures)
		: this(failures.Select(failure => new ValidationError(failure.ErrorCode, failure.ErrorMessage)))
	{
	}

	private ValidationException(IReadOnlyCollection<ValidationError> errors)
		: base(string.Join(" ", errors.Select(error => error.Message)))
	{
		Errors = errors;
	}
}
