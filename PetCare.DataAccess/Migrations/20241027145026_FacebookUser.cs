using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FacebookUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacebookUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FacebookId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookUsers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacebookUsers");
        }
    }
}
