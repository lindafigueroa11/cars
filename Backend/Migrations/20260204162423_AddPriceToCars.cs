using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename CarLocationID -> Id (lo que ya tenías)
            migrationBuilder.RenameColumn(
                name: "CarLocationID",
                table: "CarLocations",
                newName: "Id");

            // Make IsAutomatic nullable (lo que ya tenías)
            migrationBuilder.AlterColumn<bool>(
                name: "IsAutomatic",
                table: "Cars",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            // ✅ ESTO ES LO QUE FALTABA
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Cars",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove Price column
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cars");

            // Revert IsAutomatic
            migrationBuilder.AlterColumn<bool>(
                name: "IsAutomatic",
                table: "Cars",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            // Revert column rename
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CarLocations",
                newName: "CarLocationID");
        }
    }
}
