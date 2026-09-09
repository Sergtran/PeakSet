using FluentValidation;
using PeakSet.Application.Dtos;
using PeakSet.Domain.Entities;

namespace PeakSet.Application.Validators;

public sealed class CreateSessionRequestValidator : AbstractValidator<CreateSessionRequest>
{
	public CreateSessionRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("El nombre de la sesión es obligatorio.")
			.MaximumLength(WorkoutSession.MaxNameLength)
			.WithMessage($"El nombre no puede superar {WorkoutSession.MaxNameLength} caracteres.");
	}
}