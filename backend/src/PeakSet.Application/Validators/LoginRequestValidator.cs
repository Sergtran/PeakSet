using FluentValidation;
using PeakSet.Application.Dtos;

namespace PeakSet.Application.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
			.NotEmpty().WithErrorCode("EmailRequired")
			.EmailAddress().WithErrorCode("EmailInvalid");

		RuleFor(x => x.Password).NotEmpty().WithErrorCode("PasswordRequired");
    }
}
