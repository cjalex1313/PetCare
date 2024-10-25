using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpcomingVaccines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpcomingVaccines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpcomingVaccines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpcomingVaccines_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpcomingVaccines_PetId",
                table: "UpcomingVaccines",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpcomingVaccines");
        }
    }
}
