using FluentValidation;
using PeakSet.Application.Dtos;

namespace PeakSet.Application.Validators;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
	public RegisterRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
				.WithErrorCode("EmailRequired")
			.EmailAddress().WithMessage("Email is not valid.")
				.WithErrorCode("EmailInvalid");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
				.WithErrorCode("PasswordRequired")
			.MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
				.WithErrorCode("PasswordTooShort");

		RuleFor(x => x.DisplayName)
			.MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
				.WithErrorCode("NameTooLong");
	}
}
