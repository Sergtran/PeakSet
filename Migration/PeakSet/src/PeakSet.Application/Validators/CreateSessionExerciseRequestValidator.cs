using FluentValidation;
using PeakSet.Application.Dtos;
using PeakSet.Domain.Entities;

namespace PeakSet.Application.Validators;

public sealed class CreateSessionExerciseRequestValidator : AbstractValidator<CreateSessionExerciseRequest>
{
	public CreateSessionExerciseRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("El nombre del ejercicio es obligatorio.")
			.MaximumLength(SessionExercise.MaxNameLength)
			.WithMessage($"El nombre no puede superar {SessionExercise.MaxNameLength} caracteres.");

		RuleFor(x => x.ExerciseType).IsInEnum();
		RuleFor(x => x.Laterality).IsInEnum();
	}
}