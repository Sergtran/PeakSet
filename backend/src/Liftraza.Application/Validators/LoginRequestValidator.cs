using FluentValidation;
using Liftraza.Application.Dtos;

namespace Liftraza.Application.Validators;

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
