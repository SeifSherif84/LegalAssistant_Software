using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Modify_To_Decision_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appeals_DecisionId",
                table: "Appeals");

            migrationBuilder.DropIndex(
                name: "IX_Appeals_ResultDecisionId",
                table: "Appeals");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppealDate",
                table: "Appeals",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_DecisionId",
                table: "Appeals",
                column: "DecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_ResultDecisionId",
                table: "Appeals",
                column: "ResultDecisionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appeals_DecisionId",
                table: "Appeals");

            migrationBuilder.DropIndex(
                name: "IX_Appeals_ResultDecisionId",
                table: "Appeals");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppealDate",
                table: "Appeals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_DecisionId",
                table: "Appeals",
                column: "DecisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_ResultDecisionId",
                table: "Appeals",
                column: "ResultDecisionId",
                unique: true,
                filter: "[ResultDecisionId] IS NOT NULL");
        }
    }
}
