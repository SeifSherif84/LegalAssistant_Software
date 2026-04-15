using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCasePartyRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseParties_Lawyers_LawyerId",
                table: "CaseParties");

            migrationBuilder.DropIndex(
                name: "IX_CaseParties_LawyerId",
                table: "CaseParties");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParties_LawyerId",
                table: "CaseParties",
                column: "LawyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseParties_Lawyers_LawyerId",
                table: "CaseParties",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseParties_Lawyers_LawyerId",
                table: "CaseParties");

            migrationBuilder.DropIndex(
                name: "IX_CaseParties_LawyerId",
                table: "CaseParties");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParties_LawyerId",
                table: "CaseParties",
                column: "LawyerId",
                unique: true,
                filter: "[LawyerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseParties_Lawyers_LawyerId",
                table: "CaseParties",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
