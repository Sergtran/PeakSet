using FluentValidation;
using Liftraza.Application.Dtos;

namespace Liftraza.Application.Validators;

public sealed class UpdateUserSettingsValidator : AbstractValidator<UpdateUserSettingsRequest>
{
	public UpdateUserSettingsValidator()
	{
		RuleFor(x => x.Theme).IsInEnum().WithErrorCode("ThemeInvalid");

		RuleFor(x => x.TimerPrepSeconds)
			.InclusiveBetween(0, 600).WithErrorCode("SettingsPrepInvalid");

		RuleFor(x => x.TimerWorkSeconds)
			.InclusiveBetween(1, 600).WithErrorCode("SettingsWorkInvalid");

		RuleFor(x => x.TimerRestSeconds)
			.InclusiveBetween(1, 600).WithErrorCode("SettingsRestInvalid");

		RuleFor(x => x.TimerSets)
			.InclusiveBetween(1, 50).WithErrorCode("SettingsSetsInvalid");
	}
}
