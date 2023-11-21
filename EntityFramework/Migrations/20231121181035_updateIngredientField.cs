using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class updateIngredientField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalUnit",
                table: "Ingredients",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Ingredients",
                newName: "Price");

            migrationBuilder.AlterColumn<int>(
                name: "Unit",
                table: "Ingredients",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Ingredients",
                newName: "TotalUnit");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Ingredients",
                newName: "TotalPrice");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
