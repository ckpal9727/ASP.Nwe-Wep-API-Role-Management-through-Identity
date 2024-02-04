using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dummyRolr.Migrations
{
    /// <inheritdoc />
    public partial class MappinUserWithEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dfee182a-570e-4be8-814f-e0ccc8352ce7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa0a2f9b-454c-49b4-856c-e1cc86a0d52d");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "userLeaveBalances");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Leaves");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "userLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Leaves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29f4ccce-c64d-476d-b0e4-6bd29db81bfd", "1", "Admin", "Admin" },
                    { "44d40ff9-b322-4709-8236-1a19c0e33d28", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29f4ccce-c64d-476d-b0e4-6bd29db81bfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44d40ff9-b322-4709-8236-1a19c0e33d28");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "userLeaveBalances");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Leaves");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "userLeaveBalances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dfee182a-570e-4be8-814f-e0ccc8352ce7", "2", "User", "User" },
                    { "fa0a2f9b-454c-49b4-856c-e1cc86a0d52d", "1", "Admin", "Admin" }
                });
        }
    }
}
