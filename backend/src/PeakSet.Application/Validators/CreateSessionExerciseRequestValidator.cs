using FluentValidation;
using PeakSet.Application.Dtos;
using PeakSet.Domain.Entities;

namespace PeakSet.Application.Validators;

public sealed class CreateSessionExerciseRequestValidator : AbstractValidator<CreateSessionExerciseRequest>
{
	public CreateSessionExerciseRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Exercise name is required.")
				.WithErrorCode("ExerciseNameRequired")
			.MaximumLength(SessionExercise.MaxNameLength)
			.WithMessage($"Name cannot exceed {SessionExercise.MaxNameLength} characters.")
				.WithErrorCode("NameTooLong");

		RuleFor(x => x.ExerciseType).IsInEnum().WithErrorCode("ExerciseTypeInvalid");
		RuleFor(x => x.Laterality).IsInEnum().WithErrorCode("LateralityInvalid");
	}
}
