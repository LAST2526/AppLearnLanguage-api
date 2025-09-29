using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Last02.Data.Migrations
{
    /// <inheritdoc />
    public partial class seed_course : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedDate", "Deleted", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6561), false, "N5" },
                    { 2, new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6936), false, "N4" },
                    { 3, new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6938), false, "N3" },
                    { 4, new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6939), false, "N2" },
                    { 5, new DateTime(2025, 8, 3, 22, 24, 53, 585, DateTimeKind.Local).AddTicks(6941), false, "N1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
