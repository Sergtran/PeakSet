using FluentValidation;
using PeakSet.Application.Dtos;

namespace PeakSet.Application.Validators;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
	public RegisterRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Email is not valid.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

		RuleFor(x => x.DisplayName)
			.MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
	}
}