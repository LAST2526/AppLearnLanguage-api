using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Last02.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUpdateCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TitleVi",
                table: "Courses");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 7, 0, 0, 49, 362, DateTimeKind.Local).AddTicks(9399));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 7, 0, 0, 49, 362, DateTimeKind.Local).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 7, 0, 0, 49, 362, DateTimeKind.Local).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 7, 0, 0, 49, 362, DateTimeKind.Local).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 11, 7, 0, 0, 49, 362, DateTimeKind.Local).AddTicks(9828));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleVi",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Language", "TitleEn", "TitleVi" },
                values: new object[] { new DateTime(2025, 10, 30, 23, 31, 5, 364, DateTimeKind.Local).AddTicks(6023), 0, null, null });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Language", "TitleEn", "TitleVi" },
                values: new object[] { new DateTime(2025, 10, 30, 23, 31, 5, 364, DateTimeKind.Local).AddTicks(6379), 0, null, null });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Language", "TitleEn", "TitleVi" },
                values: new object[] { new DateTime(2025, 10, 30, 23, 31, 5, 364, DateTimeKind.Local).AddTicks(6381), 0, null, null });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Language", "TitleEn", "TitleVi" },
                values: new object[] { new DateTime(2025, 10, 30, 23, 31, 5, 364, DateTimeKind.Local).AddTicks(6382), 0, null, null });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Language", "TitleEn", "TitleVi" },
                values: new object[] { new DateTime(2025, 10, 30, 23, 31, 5, 364, DateTimeKind.Local).AddTicks(6384), 0, null, null });
        }
    }
}
