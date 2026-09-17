using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liftraza.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrentRoutineSince : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentRoutineSince",
                table: "UserSettings",
                type: "timestamp with time zone",
                nullable: true);

            // Existing rows get an approximation: the first workout logged with the
            // routine they currently have selected, so the card is not empty after upgrading.
            migrationBuilder.Sql(
                """
                UPDATE "UserSettings" AS s
                SET "CurrentRoutineSince" = w."FirstDate"
                FROM (
                    SELECT "UserId", "RoutineId", MIN("WorkoutDate") AS "FirstDate"
                    FROM "Workouts"
                    WHERE "RoutineId" IS NOT NULL
                    GROUP BY "UserId", "RoutineId"
                ) AS w
                WHERE s."UserId" = w."UserId"
                  AND s."CurrentRoutineId" = w."RoutineId"
                  AND s."CurrentRoutineSince" IS NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRoutineSince",
                table: "UserSettings");
        }
    }
}
