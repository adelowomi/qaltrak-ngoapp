using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NGOAPP.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusTovents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "statusId",
                table: "events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "iX_events_statusId",
                table: "events",
                column: "statusId");

            migrationBuilder.AddForeignKey(
                name: "fK_events_statuses_statusId",
                table: "events",
                column: "statusId",
                principalTable: "statuses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_events_statuses_statusId",
                table: "events");

            migrationBuilder.DropIndex(
                name: "iX_events_statusId",
                table: "events");

            migrationBuilder.DropColumn(
                name: "statusId",
                table: "events");
        }
    }
}
