using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedLocationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "CarLocations");

            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "CarLocations");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Cars",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarLocations_Cars_CarID1",
                table: "CarLocations");

            migrationBuilder.DropIndex(
                name: "IX_CarLocations_CarID1",
                table: "CarLocations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarID1",
                table: "CarLocations");

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "CarLocations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetNumber",
                table: "CarLocations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
