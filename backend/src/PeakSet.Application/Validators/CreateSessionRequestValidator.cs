using FluentValidation;
using PeakSet.Application.Dtos;
using PeakSet.Domain.Entities;

namespace PeakSet.Application.Validators;

public sealed class CreateSessionRequestValidator : AbstractValidator<CreateSessionRequest>
{
	public CreateSessionRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Session name is required.")
				.WithErrorCode("SessionNameRequired")
			.MaximumLength(WorkoutSession.MaxNameLength)
			.WithMessage($"Name cannot exceed {WorkoutSession.MaxNameLength} characters.")
				.WithErrorCode("NameTooLong");
	}
}
