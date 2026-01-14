using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Last02.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateTitleAudio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Audios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleVi",
                table: "Audios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "TitleVi",
                table: "Audios");
        }
    }
}
