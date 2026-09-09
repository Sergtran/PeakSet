using FluentValidation;
using PeakSet.Application.Dtos;
using PeakSet.Domain.Entities;

namespace PeakSet.Application.Validators;

public sealed class CreateWorkoutValidator : AbstractValidator<CreateWorkoutRequest>
{
	public CreateWorkoutValidator()
	{
		RuleFor(x => x.RoutineName).NotEmpty().MaximumLength(Routine.MaxNameLength);
		RuleFor(x => x.SessionName).NotEmpty().MaximumLength(WorkoutSession.MaxNameLength);
		RuleFor(x => x.WorkoutDate).NotEmpty();

		RuleFor(x => x.Exercises)
			.NotEmpty().WithMessage("El entrenamiento debe tener al menos un ejercicio.");

		RuleForEach(x => x.Exercises).ChildRules(exercise =>
		{
			exercise.RuleFor(e => e.Name).NotEmpty().MaximumLength(WorkoutExercise.MaxNameLength);
			exercise.RuleFor(e => e.ExerciseType).IsInEnum();
			exercise.RuleFor(e => e.Laterality).IsInEnum();
			exercise.RuleFor(e => e.Sets)
				.NotEmpty().WithMessage("Cada ejercicio debe tener al menos una serie.");

			exercise.RuleForEach(e => e.Sets).ChildRules(set =>
			{
				set.RuleFor(s => s.Reps).GreaterThanOrEqualTo(0);
				set.RuleFor(s => s.Weight).GreaterThanOrEqualTo(0);
			});
		});
	}
}
