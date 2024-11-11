using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFacebookAndGoogleIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GoogleUsers_GoogleEmail",
                table: "GoogleUsers",
                column: "GoogleEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacebookUsers_FacebookId",
                table: "FacebookUsers",
                column: "FacebookId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GoogleUsers_GoogleEmail",
                table: "GoogleUsers");

            migrationBuilder.DropIndex(
                name: "IX_FacebookUsers_FacebookId",
                table: "FacebookUsers");
        }
    }
}
