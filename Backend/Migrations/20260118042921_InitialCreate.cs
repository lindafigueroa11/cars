using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarLocations_Cars_CarID1",
                table: "CarLocations");

            migrationBuilder.DropIndex(
                name: "IX_CarLocations_CarID1",
                table: "CarLocations");

            migrationBuilder.DropColumn(
                name: "CarID1",
                table: "CarLocations");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Cars",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAutomatic",
                table: "Cars",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Cars",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "IsAutomatic",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "CarID1",
                table: "CarLocations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarLocations_CarID1",
                table: "CarLocations",
                column: "CarID1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarLocations_Cars_CarID1",
                table: "CarLocations",
                column: "CarID1",
                principalTable: "Cars",
                principalColumn: "CarID");
        }
    }
}
