using FluentValidation;
using PeakSet.Application.Dtos;
using PeakSet.Domain.Entities;

namespace PeakSet.Application.Validators;

public sealed class CreateRoutineValidator : AbstractValidator<CreateRoutineRequest>
{
	public CreateRoutineValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Routine name is required.")
			.MaximumLength(Routine.MaxNameLength)
			.WithMessage($"Name cannot exceed {Routine.MaxNameLength} characters.");
	}
}