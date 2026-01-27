using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewDBModifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appeals_Cases_CaseId",
                table: "Appeals");

            migrationBuilder.DropForeignKey(
                name: "FK_CourtSessions_Decisions_DecisionId",
                table: "CourtSessions");

            migrationBuilder.DropIndex(
                name: "IX_CourtSessions_DecisionId",
                table: "CourtSessions");

            migrationBuilder.DropColumn(
                name: "DecisionId",
                table: "CourtSessions");

            migrationBuilder.RenameColumn(
                name: "DecisionText",
                table: "Decisions",
                newName: "SentenceText");

            migrationBuilder.RenameColumn(
                name: "FiledDate",
                table: "Appeals",
                newName: "AppealDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Persons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Documents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnalyzedAt",
                table: "Documents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtractedTextPath",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnalyzedByAI",
                table: "Documents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LawyerId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DecisionType",
                table: "Decisions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CourtSessionId",
                table: "Decisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalVerdict",
                table: "Decisions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JudgeName",
                table: "Decisions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SentenceType",
                table: "Decisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Cases",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CrimeCategory",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Jurisdiction",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "crimeType",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "CaseParties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Appeals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AppealType",
                table: "Appeals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AppealingPartyId",
                table: "Appeals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppellantSide",
                table: "Appeals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DecisionId",
                table: "Appeals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LawyerId",
                table: "Appeals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Appeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Outcome",
                table: "Appeals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Appeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ResultDecisionId",
                table: "Appeals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AiAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    VulnerabilitiesSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    AnalysisType = table.Column<int>(type: "int", nullable: false),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiAnalyses_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AiAnalyses_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LawyerId",
                table: "Documents",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_CourtSessionId",
                table: "Decisions",
                column: "CourtSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_AppealingPartyId",
                table: "Appeals",
                column: "AppealingPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_DecisionId",
                table: "Appeals",
                column: "DecisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_LawyerId",
                table: "Appeals",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_ResultDecisionId",
                table: "Appeals",
                column: "ResultDecisionId",
                unique: true,
                filter: "[ResultDecisionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AiAnalyses_CaseId",
                table: "AiAnalyses",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AiAnalyses_DocumentId",
                table: "AiAnalyses",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appeals_CaseParties_AppealingPartyId",
                table: "Appeals",
                column: "AppealingPartyId",
                principalTable: "CaseParties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appeals_Cases_CaseId",
                table: "Appeals",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appeals_Decisions_DecisionId",
                table: "Appeals",
                column: "DecisionId",
                principalTable: "Decisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appeals_Decisions_ResultDecisionId",
                table: "Appeals",
                column: "ResultDecisionId",
                principalTable: "Decisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appeals_Lawyers_LawyerId",
                table: "Appeals",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_CourtSessions_CourtSessionId",
                table: "Decisions",
                column: "CourtSessionId",
                principalTable: "CourtSessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Lawyers_LawyerId",
                table: "Documents",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appeals_CaseParties_AppealingPartyId",
                table: "Appeals");

            migrationBuilder.DropForeignKey(
                name: "FK_Appeals_Cases_CaseId",
                table: "Appeals");

            migrationBuilder.DropForeignKey(
                name: "FK_Appeals_Decisions_DecisionId",
                table: "Appeals");

            migrationBuilder.DropForeignKey(
                name: "FK_Appeals_Decisions_ResultDecisionId",
                table: "Appeals");

            migrationBuilder.DropForeignKey(
                name: "FK_Appeals_Lawyers_LawyerId",
                table: "Appeals");

            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_CourtSessions_CourtSessionId",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Lawyers_LawyerId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "AiAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_Documents_LawyerId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_CourtSessionId",
                table: "Decisions");

            migrationBuilder.DropIndex(
                name: "IX_Appeals_AppealingPartyId",
                table: "Appeals");

            migrationBuilder.DropIndex(
                name: "IX_Appeals_DecisionId",
                table: "Appeals");

            migrationBuilder.DropIndex(
                name: "IX_Appeals_LawyerId",
                table: "Appeals");

            migrationBuilder.DropIndex(
                name: "IX_Appeals_ResultDecisionId",
                table: "Appeals");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "AnalyzedAt",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ExtractedTextPath",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsAnalyzedByAI",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LawyerId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CourtSessionId",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "IsFinalVerdict",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "JudgeName",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "SentenceType",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "CrimeCategory",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Jurisdiction",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "crimeType",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "AppealingPartyId",
                table: "Appeals");

            migrationBuilder.DropColumn(
                name: "AppellantSide",
                table: "Appeals");

            migrationBuilder.DropColumn(
                name: "DecisionId",
                table: "Appeals");

            migrationBuilder.DropColumn(
                name: "LawyerId",
                table: "Appeals");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Appeals");

            migrationBuilder.DropColumn(
                name: "Outcome",
                table: "Appeals");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Appeals");

            migrationBuilder.DropColumn(
                name: "ResultDecisionId",
                table: "Appeals");

            migrationBuilder.RenameColumn(
                name: "SentenceText",
                table: "Decisions",
                newName: "DecisionText");

            migrationBuilder.RenameColumn(
                name: "AppealDate",
                table: "Appeals",
                newName: "FiledDate");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DecisionType",
                table: "Decisions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DecisionId",
                table: "CourtSessions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Cases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "CaseParties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appeals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AppealType",
                table: "Appeals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CourtSessions_DecisionId",
                table: "CourtSessions",
                column: "DecisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appeals_Cases_CaseId",
                table: "Appeals",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourtSessions_Decisions_DecisionId",
                table: "CourtSessions",
                column: "DecisionId",
                principalTable: "Decisions",
                principalColumn: "Id");
        }
    }
}
