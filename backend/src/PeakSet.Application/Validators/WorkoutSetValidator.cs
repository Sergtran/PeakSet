using FluentValidation;
using PeakSet.Application.Dtos;

public sealed class WorkoutSetValidator : AbstractValidator<CreateWorkoutSetRequest>
{
	public WorkoutSetValidator()
	{
		RuleFor(x => x.Reps).GreaterThanOrEqualTo(0);
		RuleFor(x => x.Weight).GreaterThanOrEqualTo(0);
	}
}