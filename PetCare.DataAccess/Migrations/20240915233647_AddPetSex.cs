﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPetSex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "Pets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Pets");
        }
    }
}
