using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class VaccineNextDueDateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Vaccines_NextDueDate",
                table: "Vaccines",
                column: "NextDueDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vaccines_NextDueDate",
                table: "Vaccines");
        }
    }
}
