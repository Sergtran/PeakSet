using FluentValidation;
using PeakSet.Application.Dtos;

namespace PeakSet.Application.Validators;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
	public RegisterRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("El email es obligatorio.")
			.EmailAddress().WithMessage("El email no es válido.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("La contraseña es obligatoria.")
			.MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

		RuleFor(x => x.DisplayName)
			.MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");
	}
}