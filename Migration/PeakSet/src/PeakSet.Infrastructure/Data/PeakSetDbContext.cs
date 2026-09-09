using PeakSet.Domain.Entities;
using PeakSet.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeakSet.Domain.ValueObjects;

namespace PeakSet.Infrastructure.Data;

public sealed class PeakSetDbContext : IdentityDbContext<ApplicationUser>
{
    public PeakSetDbContext(DbContextOptions<PeakSetDbContext> options)
        : base(options)
    {
    }

    public DbSet<Routine> Routines { get; set; } = null!;
    public DbSet<Workout> Workouts { get; set; } = null!;
    public DbSet<ExerciseCatalogEntry> ExerciseCatalogEntries { get; set; } = null!;
    public DbSet<UserSettings> UserSettings { get; set; } = null!;
    public DbSet<ExerciseNote> ExerciseNotes { get; set; } = null!;
    public DbSet<UserExercisePreference> UserExercisePreferences { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

		// Routine
		modelBuilder.Entity<Routine>(e =>
		{
			e.Property(r => r.Name)
			 .HasConversion(name => name.Value, value => new Name(value))
			 .HasMaxLength(Routine.MaxNameLength)
			 .IsRequired();

			e.HasIndex(r => r.UserId);

			e.HasOne<ApplicationUser>()
			 .WithMany()
			 .HasForeignKey(r => r.UserId)
			 .OnDelete(DeleteBehavior.Cascade);

			e.HasMany(r => r.Sessions)
			 .WithOne()
			 .HasForeignKey(s => s.RoutineId)
			 .OnDelete(DeleteBehavior.Cascade);
		});

		// WorkoutSession
		modelBuilder.Entity<WorkoutSession>(e =>
		{
			e.Property(s => s.Name)
			 .HasConversion(name => name.Value, value => new Name(value))
			 .HasMaxLength(WorkoutSession.MaxNameLength)
			 .IsRequired();

			e.HasMany(s => s.Exercises).WithOne().HasForeignKey(x => x.WorkoutSessionId).OnDelete(DeleteBehavior.Cascade);
		});

		// SessionExercise
		modelBuilder.Entity<SessionExercise>(e =>
		{
			e.Property(x => x.Name)
			 .HasConversion(name => name.Value, value => new Name(value))
			 .HasMaxLength(SessionExercise.MaxNameLength)
			 .IsRequired();

			e.Property(x => x.ExerciseType).HasConversion<string>();
			e.Property(x => x.Laterality).HasConversion<string>();

			e.HasIndex(x => x.WorkoutSessionId);
		});

		// Workout
		modelBuilder.Entity<Workout>(e =>
		{
			e.Property(w => w.RoutineName)
			 .HasConversion(name => name.Value, value => new Name(value))
			 .HasMaxLength(Routine.MaxNameLength)
			 .IsRequired();

			e.Property(w => w.SessionName)
			 .HasConversion(name => name.Value, value => new Name(value))
			 .HasMaxLength(WorkoutSession.MaxNameLength)
			 .IsRequired();

			e.HasOne<Routine>()
			 .WithMany()
			 .HasForeignKey(w => w.RoutineId)
			 .OnDelete(DeleteBehavior.Restrict);

			e.HasOne<ApplicationUser>()
			 .WithMany()
			 .HasForeignKey(w => w.UserId)
			 .OnDelete(DeleteBehavior.Cascade);

			e.HasIndex(w => new { w.UserId, w.WorkoutDate });

			e.HasMany(w => w.Exercises).WithOne().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.Cascade);
		});

		// WorkoutExercise
		modelBuilder.Entity<WorkoutExercise>(e =>
		{
			e.Property(x => x.Name)
			 .HasConversion(name => name.Value, value => new Name(value))
			 .HasMaxLength(WorkoutExercise.MaxNameLength)
			 .IsRequired();

			e.Property(x => x.ExerciseType).HasConversion<string>();
			e.Property(x => x.Laterality).HasConversion<string>();
			e.Property(x => x.PrStatus).HasConversion<string?>();

			e.HasIndex(x => x.WorkoutId);

			e.HasMany(x => x.Sets).WithOne().HasForeignKey(s => s.WorkoutExerciseId).OnDelete(DeleteBehavior.Cascade);
		});

		// WorkoutSet
		modelBuilder.Entity<WorkoutSet>(e =>
		{
			e.Property(s => s.SetNumber).IsRequired();

			e.Property(s => s.Reps)
			 .HasConversion(r => r.Value, v => new Repetitions(v));

			e.Property(s => s.Weight)
			 .HasConversion(w => w.Value, v => new Weight(v));

			e.ToTable(t => t.HasCheckConstraint("CK_WorkoutSets_SetNumber_Positive", "\"SetNumber\" > 0"));
			e.ToTable(t => t.HasCheckConstraint("CK_WorkoutSets_Reps_NonNegative", "\"Reps\" >= 0"));
			e.ToTable(t => t.HasCheckConstraint("CK_WorkoutSets_Weight_NonNegative", "\"Weight\" >= 0"));
		});

		// ExerciseCatalogEntry (configuración + seed HasData en clase separada)
		modelBuilder.ApplyConfiguration(new ExerciseCatalogConfiguration());

		// ExerciseNote
		modelBuilder.Entity<ExerciseNote>(e =>
		{
			e.Property(x => x.ExerciseName)
			 .HasConversion(name => name.Value, value => new Name(value))
			 .HasMaxLength(150)
			 .IsRequired();

			e.HasIndex(x => new { x.UserId, x.ExerciseName }).IsUnique();

			e.HasOne<ApplicationUser>()
			 .WithMany()
			 .HasForeignKey(x => x.UserId)
			 .OnDelete(DeleteBehavior.Cascade);
		});

		// UserExercisePreference
		modelBuilder.Entity<UserExercisePreference>(e =>
		{
			e.Property(x => x.ExerciseName)
			 .HasConversion(name => name.Value, value => new Name(value))
			 .HasMaxLength(150)
			 .IsRequired();

			e.Property(x => x.ExerciseType).HasConversion<string>();
			e.Property(x => x.Laterality).HasConversion<string>();

			e.HasIndex(x => new { x.UserId, x.ExerciseName }).IsUnique();

			e.HasOne<ApplicationUser>()
			 .WithMany()
			 .HasForeignKey(x => x.UserId)
			 .OnDelete(DeleteBehavior.Cascade);
		});

		// UserSettings
		modelBuilder.Entity<UserSettings>(e =>
		{
			e.Property(x => x.Theme).HasConversion<string>();

			e.HasIndex(x => x.UserId).IsUnique();

			e.HasOne<ApplicationUser>()
			 .WithMany()
			 .HasForeignKey(x => x.UserId)
			 .OnDelete(DeleteBehavior.Cascade);

			e.HasOne<Routine>()
			 .WithMany()
			 .HasForeignKey(x => x.CurrentRoutineId)
			 .OnDelete(DeleteBehavior.SetNull);
		});

		// ApplicationUser
		modelBuilder.Entity<ApplicationUser>(e =>
		{
			e.HasIndex(u => u.Email).IsUnique();
		});
	}
}
