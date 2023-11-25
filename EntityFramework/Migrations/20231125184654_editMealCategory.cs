using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class editMealCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c9f44619-7682-4931-a523-77909074da61"));

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Meals");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateOfBirth", "Email", "EmployeeId", "FirstName", "Image", "IsDeleted", "LastName", "Password", "Role", "Sex", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("8d9a21e1-c8c4-4087-a4ac-9790c8a65a88"), new DateTime(2023, 11, 26, 1, 46, 54, 579, DateTimeKind.Local).AddTicks(4358), null, new DateTime(2023, 11, 25, 18, 46, 54, 579, DateTimeKind.Utc).AddTicks(4356), "admin@gmail.com", "Ad1", "admin", "", 0, "", "admin123", 0, 1, 1, new DateTime(2023, 11, 26, 1, 46, 54, 579, DateTimeKind.Local).AddTicks(4359), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8d9a21e1-c8c4-4087-a4ac-9790c8a65a88"));

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Meals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateOfBirth", "Email", "EmployeeId", "FirstName", "Image", "IsDeleted", "LastName", "Password", "Role", "Sex", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("c9f44619-7682-4931-a523-77909074da61"), new DateTime(2023, 11, 26, 1, 29, 27, 72, DateTimeKind.Local).AddTicks(9717), null, new DateTime(2023, 11, 25, 18, 29, 27, 72, DateTimeKind.Utc).AddTicks(9715), "admin@gmail.com", "Ad1", "admin", "", 0, "", "admin123", 0, 1, 1, new DateTime(2023, 11, 26, 1, 29, 27, 72, DateTimeKind.Local).AddTicks(9717), null });
        }
    }
}
