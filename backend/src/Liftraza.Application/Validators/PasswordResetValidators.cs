using FluentValidation;
using Liftraza.Application.Dtos;

namespace Liftraza.Application.Validators;

public sealed class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
	public ForgotPasswordRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithErrorCode("EmailRequired")
			.EmailAddress().WithErrorCode("EmailInvalid");
	}
}

public sealed class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
	public ResetPasswordRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithErrorCode("EmailRequired")
			.EmailAddress().WithErrorCode("EmailInvalid");

		RuleFor(x => x.Token)
			.NotEmpty().WithErrorCode("ResetTokenRequired");

		RuleFor(x => x.NewPassword)
			.NotEmpty().WithErrorCode("PasswordRequired")
			.MinimumLength(6).WithErrorCode("PasswordTooShort");
	}
}
