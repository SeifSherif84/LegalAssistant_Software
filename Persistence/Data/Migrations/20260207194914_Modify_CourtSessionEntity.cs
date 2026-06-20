using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class Modify_CourtSessionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "CourtSessions",
                newName: "JudgeName");

            migrationBuilder.AddColumn<string>(
                name: "AdjournmentReason",
                table: "CourtSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledReason",
                table: "CourtSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourtName",
                table: "CourtSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourtRoom",
                table: "CourtSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "CourtSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NextSessionId",
                table: "CourtSessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderDate",
                table: "CourtSessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionStatus",
                table: "CourtSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourtSessions_NextSessionId",
                table: "CourtSessions",
                column: "NextSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourtSessions_CourtSessions_NextSessionId",
                table: "CourtSessions",
                column: "NextSessionId",
                principalTable: "CourtSessions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourtSessions_CourtSessions_NextSessionId",
                table: "CourtSessions");

            migrationBuilder.DropIndex(
                name: "IX_CourtSessions_NextSessionId",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "AdjournmentReason",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "CancelledReason",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "CourtName",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "CourtRoom",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "NextSessionId",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "ReminderDate",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "SessionStatus",
                table: "CourtSessions");

            migrationBuilder.RenameColumn(
                name: "JudgeName",
                table: "CourtSessions",
                newName: "Location");
        }
    }
}
