using FluentValidation;
using Liftraza.Application.Dtos;

public sealed class WorkoutSetValidator : AbstractValidator<CreateWorkoutSetRequest>
{
	public WorkoutSetValidator()
	{
		RuleFor(x => x.Reps).GreaterThanOrEqualTo(0).WithErrorCode("SetRepsInvalid");
		RuleFor(x => x.Weight).GreaterThanOrEqualTo(0).WithErrorCode("SetWeightInvalid");
	}
}
