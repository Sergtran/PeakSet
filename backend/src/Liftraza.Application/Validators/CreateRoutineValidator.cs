using FluentValidation;
using Liftraza.Application.Dtos;
using Liftraza.Domain.Entities;

namespace Liftraza.Application.Validators;

public sealed class CreateRoutineValidator : AbstractValidator<CreateRoutineRequest>
{
	public CreateRoutineValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Routine name is required.")
				.WithErrorCode("RoutineNameRequired")
			.MaximumLength(Routine.MaxNameLength)
			.WithMessage($"Name cannot exceed {Routine.MaxNameLength} characters.")
				.WithErrorCode("NameTooLong");
	}
}
