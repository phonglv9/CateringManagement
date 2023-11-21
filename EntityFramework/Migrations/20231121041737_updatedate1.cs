using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class updatedate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientImports_Ingredients_IngredientId",
                table: "IngredientImports");

            migrationBuilder.DropForeignKey(
                name: "FK_MealIngredients_Meals_MealId",
                table: "MealIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Ingredients_IngredientId",
                table: "Meals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrderDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())");

            migrationBuilder.AlterColumn<Guid>(
                name: "IngredientId",
                table: "Meals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MealId",
                table: "MealIngredients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IngredientId",
                table: "IngredientImports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientImports_Ingredients_IngredientId",
                table: "IngredientImports",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealIngredients_Meals_MealId",
                table: "MealIngredients",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Ingredients_IngredientId",
                table: "Meals",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientImports_Ingredients_IngredientId",
                table: "IngredientImports");

            migrationBuilder.DropForeignKey(
                name: "FK_MealIngredients_Meals_MealId",
                table: "MealIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Ingredients_IngredientId",
                table: "Meals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "IngredientId",
                table: "Meals",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MealId",
                table: "MealIngredients",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "IngredientId",
                table: "IngredientImports",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                columns: new[] { "OrderId", "MealId" });

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientImports_Ingredients_IngredientId",
                table: "IngredientImports",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealIngredients_Meals_MealId",
                table: "MealIngredients",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Ingredients_IngredientId",
                table: "Meals",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id");
        }
    }
}
