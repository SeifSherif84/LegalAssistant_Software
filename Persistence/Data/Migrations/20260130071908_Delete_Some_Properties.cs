using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class Delete_Some_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtractedTextPath",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "NextHearingDate",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "VerdictDate",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "VulnerabilitiesSummary",
                table: "AiAnalyses",
                newName: "Vulnerabilities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vulnerabilities",
                table: "AiAnalyses",
                newName: "VulnerabilitiesSummary");

            migrationBuilder.AddColumn<string>(
                name: "ExtractedTextPath",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextHearingDate",
                table: "Cases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerdictDate",
                table: "Cases",
                type: "datetime2",
                nullable: true);
        }
    }
}
