using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCasePartyRelationships2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CaseParties_PersonId",
                table: "CaseParties");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParties_PersonId",
                table: "CaseParties",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CaseParties_PersonId",
                table: "CaseParties");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParties_PersonId",
                table: "CaseParties",
                column: "PersonId",
                unique: true);
        }
    }
}
