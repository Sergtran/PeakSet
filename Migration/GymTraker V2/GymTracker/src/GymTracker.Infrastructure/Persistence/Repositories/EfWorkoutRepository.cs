using GymTracker.Application.Abstractions.Repositories;
using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;
using GymTracker.Domain.ValueObjects;
using GymTracker.Infrastructure.Data;
using GymTracker.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Persistence.Repositories;

public sealed class EfWorkoutRepository	: IWorkoutRepository
{
	private readonly GymTrackerDbContext _db;

	public EfWorkoutRepository(GymTrackerDbContext db)
		=> _db = db;

	public async Task<Workout?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default)
		=> await _db.Workouts
			.AsNoTracking()
			.AsSplitQuery()
			.Include(w => w.Exercises)
				.ThenInclude(e => e.Sets)
			.FirstOrDefaultAsync(w => w.UserId == userId && w.Id == id, ct);

	public async Task<(IReadOnlyList<Workout> Items, int TotalCount)> GetByUserPagedAsync(
		string userId, int page, int pageSize, CancellationToken ct = default)
	{
		var query = _db.Workouts
			.Where(w => w.UserId == userId)
			.OrderByDescending(w => w.WorkoutDate)
			.ThenByDescending(w => w.CreatedAt);

		var totalCount = await query.CountAsync(ct);

		var items = await query
			.AsSplitQuery()
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Include(w => w.Exercises)
				.ThenInclude(e => e.Sets)
			.ToListAsync(ct);

		return (items, totalCount);
	}

    public async Task<decimal?> GetPreviousBestMetricAsync(
        string userId, string exerciseName, Laterality laterality,
        ExerciseType type, Guid? excludeWorkoutId = null, CancellationToken ct = default)
    {
        var sets = await _db.Workouts
            .Where(w => w.UserId == userId
                && (!excludeWorkoutId.HasValue || w.Id != excludeWorkoutId.Value))
            .SelectMany(w => w.Exercises)
            .Where(e => e.Name == new Name(exerciseName) && e.Laterality == laterality)
            .SelectMany(e => e.Sets)
            .Select(s => new { Reps = s.Reps, Weight = s.Weight })
            .ToListAsync(ct);

        if (sets.Count == 0)
            return null;

        return type == ExerciseType.Bodyweight
            ? sets.Max(s => (decimal)s.Reps.Value)
            : sets.Max(s => s.Weight.Value);
    }

	public async Task AddAsync(Workout workout, CancellationToken ct = default)
	{
		_db.Workouts.Add(workout);
		await _db.SaveChangesAsync(ct);
	}

	public async Task<RoutineWorkoutSummary?> GetRoutineSummaryAsync(
		string userId, Guid routineId, CancellationToken ct = default)
	{
		var query = _db.Workouts.Where(w => w.UserId == userId && w.RoutineId == routineId);

		var count = await query.CountAsync(ct);
		if (count == 0)
			return null;

		var first = await query.MinAsync(w => (DateTime?)w.WorkoutDate, ct);
		var last = await query.MaxAsync(w => (DateTime?)w.WorkoutDate, ct);
		var prCount = await query
			.SelectMany(w => w.Exercises)
			.CountAsync(e => e.PrStatus == PrStatus.New, ct);

		return new RoutineWorkoutSummary(count, first, last, prCount);
	}

	public async Task<IReadOnlyList<DateTime>> GetWorkoutDatesAsync(
		string userId, Guid routineId, CancellationToken ct = default)
		=> await _db.Workouts
			.Where(w => w.UserId == userId && w.RoutineId == routineId)
			.Select(w => w.WorkoutDate)
			.ToListAsync(ct);

	public async Task<IReadOnlyList<ExerciseFrequency>> GetExerciseFrequencyAsync(
		string userId, Guid routineId, int limit, CancellationToken ct = default)
	{
		var rows = await _db.Workouts
			.Where(w => w.UserId == userId && w.RoutineId == routineId)
			.SelectMany(w => w.Exercises)
			.GroupBy(e => e.Name)
			.OrderByDescending(g => g.Count())
			.Take(limit)
			.Select(g => new { Name = g.Key, Count = g.Count() })
			.ToListAsync(ct);

		return rows.Select(r => new ExerciseFrequency(r.Name.Value, r.Count)).ToList();
	}

	public async Task<IReadOnlyList<ExerciseSummary>> GetUserExercisesAsync(
		string userId, CancellationToken ct = default)
	{
		// Agregación en SQL: GROUP BY sobre el nombre (converter) con COUNT(*) y MAX(WorkoutDate).
		var rows = await _db.Workouts
			.Where(w => w.UserId == userId)
			.SelectMany(w => w.Exercises, (w, e) => new { Exercise = e, WorkoutDate = w.WorkoutDate })
			.GroupBy(x => x.Exercise.Name)
			.Select(g => new
			{
				Name = g.Key,                       // Name convertido: EF agrupa por la columna
				SessionCount = g.Count(),
				LastUsed = g.Max(x => x.WorkoutDate)
			})
			.OrderByDescending(x => x.LastUsed)
			.ToListAsync(ct);

		return rows
			.Select(x => new ExerciseSummary(x.Name.Value, x.SessionCount, x.LastUsed))
			.ToList();
	}

	public async Task<IReadOnlyList<ExerciseSessionRow>> GetExerciseSessionRowsAsync(
		string userId, string exerciseName, CancellationToken ct = default)
	{
		var rows = await _db.Workouts
			.Where(w => w.UserId == userId)
			.SelectMany(w => w.Exercises
				.Where(e => e.Name == new Name(exerciseName))
				.Select(e => new
				{
					WorkoutDate = w.WorkoutDate,
					ExerciseType = e.ExerciseType,
					Laterality = e.Laterality,
					PrStatus = e.PrStatus,
					// Se proyectan los Value Objects completos (converter) y se mapea .Value en memoria:
					// EF Core no traduce .Value sobre propiedades convertidas dentro de la query.
					Sets = e.Sets.Select(s => new { Reps = s.Reps, Weight = s.Weight })
				}))
			.ToListAsync(ct);

		return rows
			.Select(r => new ExerciseSessionRow(
				r.WorkoutDate,
				r.ExerciseType,
				r.Laterality,
				r.PrStatus,
				r.Sets.Select(s => (s.Reps.Value, s.Weight.Value)).ToList()
			))
			.ToList();
	}

	public async Task<IReadOnlyList<CalendarWorkoutRow>> GetCalendarWorkoutsAsync(
		string userId, DateTime fromInclusive, DateTime toExclusive, CancellationToken ct = default)
	{
		var rows = await _db.Workouts
			.AsNoTracking()
			.Where(w => w.UserId == userId
				&& w.WorkoutDate >= fromInclusive
				&& w.WorkoutDate < toExclusive)
			.OrderBy(w => w.WorkoutDate)
			.Select(w => new { w.Id, w.WorkoutDate, Routine = w.RoutineName, Session = w.SessionName })
			.ToListAsync(ct);

		return rows
			.Select(r => new CalendarWorkoutRow(r.Id, r.WorkoutDate, r.Routine.Value, r.Session.Value))
			.ToList();
	}

	public async Task<Workout?> GetByIdForUpdateAsync(
		string userId, Guid id, CancellationToken ct = default)
		=> await _db.Workouts
			.AsTracking()
			.FirstOrDefaultAsync(w => w.UserId == userId && w.Id == id, ct);

	public async Task UpdateAsync(Workout workout, CancellationToken ct = default)
		=> await _db.SaveChangesAsync(ct);

	public async Task DeleteAsync(Workout workout, CancellationToken ct = default)
	{
		_db.Workouts.Remove(workout);
		await _db.SaveChangesAsync(ct);
	}

	public async Task<IReadOnlyList<Workout>> GetByUserAllAsync(
	string userId, CancellationToken ct = default)
	=> await _db.Workouts
		.AsNoTracking()
		.AsSplitQuery()
		.Where(w => w.UserId == userId)
		.OrderBy(w => w.WorkoutDate)
		.Include(w => w.Exercises)
			.ThenInclude(e => e.Sets)
		.ToListAsync(ct);
}
