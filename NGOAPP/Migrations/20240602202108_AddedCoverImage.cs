using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NGOAPP.Migrations
{
    /// <inheritdoc />
    public partial class AddedCoverImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "coverImage",
                table: "events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coverImage",
                table: "events");
        }
    }
}
