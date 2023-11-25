using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class editMeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e0214e99-c6c3-4018-bc58-938b3ee99502"));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("e0214e99-c6c3-4018-bc58-938b3ee99502"), new DateTime(2023, 11, 26, 0, 45, 50, 945, DateTimeKind.Local).AddTicks(9551), null, new DateTime(2023, 11, 25, 17, 45, 50, 945, DateTimeKind.Utc).AddTicks(9548), "admin@gmail.com", "Ad1", "admin", "", 0, "", "admin123", 0, 1, 1, new DateTime(2023, 11, 26, 0, 45, 50, 945, DateTimeKind.Local).AddTicks(9552), null });
        }
    }
}
