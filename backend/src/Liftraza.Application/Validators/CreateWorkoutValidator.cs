using FluentValidation;
using Liftraza.Application.Dtos;
using Liftraza.Domain.Entities;

namespace Liftraza.Application.Validators;

public sealed class CreateWorkoutValidator : AbstractValidator<CreateWorkoutRequest>
{
	public CreateWorkoutValidator()
	{
		RuleFor(x => x.RoutineName)
			.NotEmpty().WithErrorCode("RoutineNameRequired")
			.MaximumLength(Routine.MaxNameLength).WithErrorCode("NameTooLong");

		RuleFor(x => x.SessionName)
			.NotEmpty().WithErrorCode("SessionNameRequired")
			.MaximumLength(WorkoutSession.MaxNameLength).WithErrorCode("NameTooLong");

		RuleFor(x => x.WorkoutDate).NotEmpty().WithErrorCode("WorkoutDateRequired");

		RuleFor(x => x.Exercises)
			.NotEmpty().WithMessage("A workout must have at least one exercise.")
				.WithErrorCode("WorkoutExercisesRequired");

		RuleForEach(x => x.Exercises).ChildRules(exercise =>
		{
			exercise.RuleFor(e => e.Name)
				.NotEmpty().WithErrorCode("ExerciseNameRequired")
				.MaximumLength(WorkoutExercise.MaxNameLength).WithErrorCode("NameTooLong");

			exercise.RuleFor(e => e.ExerciseType).IsInEnum().WithErrorCode("ExerciseTypeInvalid");
			exercise.RuleFor(e => e.Laterality).IsInEnum().WithErrorCode("LateralityInvalid");

			exercise.RuleFor(e => e.Sets)
				.NotEmpty().WithMessage("Each exercise must have at least one set.")
					.WithErrorCode("ExerciseSetsRequired");

			exercise.RuleForEach(e => e.Sets).ChildRules(set =>
			{
				set.RuleFor(s => s.Reps).GreaterThanOrEqualTo(0).WithErrorCode("SetRepsInvalid");
				set.RuleFor(s => s.Weight).GreaterThanOrEqualTo(0).WithErrorCode("SetWeightInvalid");
			});
		});
	}
}
