using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakSet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTrainingCyclesAddCurrentRoutine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedTrainingCycles");

            migrationBuilder.DropTable(
                name: "TrainingCycles");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentRoutineId",
                table: "UserSettings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_CurrentRoutineId",
                table: "UserSettings",
                column: "CurrentRoutineId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Routines_CurrentRoutineId",
                table: "UserSettings",
                column: "CurrentRoutineId",
                principalTable: "Routines",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Routines_CurrentRoutineId",
                table: "UserSettings");

            migrationBuilder.DropIndex(
                name: "IX_UserSettings_CurrentRoutineId",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "CurrentRoutineId",
                table: "UserSettings");

            migrationBuilder.CreateTable(
                name: "CompletedTrainingCycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoutineName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    WeeksCompleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedTrainingCycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedTrainingCycles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingCycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentWeek = table.Column<int>(type: "integer", nullable: false),
                    RoutineId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalWeeks = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCycles", x => x.Id);
                    table.CheckConstraint("CK_TrainingCycles_CurrentWeek_Range", "\"CurrentWeek\" >= 1 AND \"CurrentWeek\" <= \"TotalWeeks\"");
                    table.ForeignKey(
                        name: "FK_TrainingCycles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingCycles_Routines_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "Routines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTrainingCycles_UserId",
                table: "CompletedTrainingCycles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCycles_RoutineId",
                table: "TrainingCycles",
                column: "RoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCycles_UserId",
                table: "TrainingCycles",
                column: "UserId",
                unique: true);
        }
    }
}
