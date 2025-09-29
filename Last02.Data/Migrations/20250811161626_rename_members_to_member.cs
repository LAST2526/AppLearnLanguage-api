using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Last02.Data.Migrations
{
    /// <inheritdoc />
    public partial class rename_members_to_member : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1614));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1980));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1983));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1984));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 16, 26, 31, DateTimeKind.Local).AddTicks(1986));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(5887));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(6237));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(6240));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(6241));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 11, 23, 4, 3, 743, DateTimeKind.Local).AddTicks(6243));
        }
    }
}
