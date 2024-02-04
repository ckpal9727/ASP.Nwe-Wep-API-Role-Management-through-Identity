using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dummyRolr.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40d36c90-7d15-4f6b-8f5d-04f2f580151e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c141f39a-63af-47c1-96fe-d8dd795882cf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "06f4b970-e566-4549-b359-7221cffc9ad5", "2", "User", "User" },
                    { "2a916ec3-a51b-4d88-af15-ac96fddc6322", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06f4b970-e566-4549-b359-7221cffc9ad5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a916ec3-a51b-4d88-af15-ac96fddc6322");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "40d36c90-7d15-4f6b-8f5d-04f2f580151e", "1", "Admin", "Admin" },
                    { "c141f39a-63af-47c1-96fe-d8dd795882cf", "2", "User", "User" }
                });
        }
    }
}
