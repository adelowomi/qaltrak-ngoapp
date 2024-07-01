using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NGOAPP.Migrations
{
    /// <inheritdoc />
    public partial class AddedColorToAdminSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "adminSchedules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "adminSchedules",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "adminSchedules");

            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "adminSchedules");
        }
    }
}
