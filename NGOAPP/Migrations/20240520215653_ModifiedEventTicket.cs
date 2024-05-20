using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NGOAPP.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedEventTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_eventTickets_ticketTypes_ticketTypeId1",
                table: "eventTickets");

            migrationBuilder.DropIndex(
                name: "iX_eventTickets_ticketTypeId1",
                table: "eventTickets");

            migrationBuilder.DropColumn(
                name: "ticketTypeId1",
                table: "eventTickets");

            migrationBuilder.AlterColumn<int>(
                name: "ticketTypeId",
                table: "eventTickets",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "iX_eventTickets_ticketTypeId",
                table: "eventTickets",
                column: "ticketTypeId");

            migrationBuilder.AddForeignKey(
                name: "fK_eventTickets_ticketTypes_ticketTypeId",
                table: "eventTickets",
                column: "ticketTypeId",
                principalTable: "ticketTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_eventTickets_ticketTypes_ticketTypeId",
                table: "eventTickets");

            migrationBuilder.DropIndex(
                name: "iX_eventTickets_ticketTypeId",
                table: "eventTickets");

            migrationBuilder.AlterColumn<Guid>(
                name: "ticketTypeId",
                table: "eventTickets",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ticketTypeId1",
                table: "eventTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "iX_eventTickets_ticketTypeId1",
                table: "eventTickets",
                column: "ticketTypeId1");

            migrationBuilder.AddForeignKey(
                name: "fK_eventTickets_ticketTypes_ticketTypeId1",
                table: "eventTickets",
                column: "ticketTypeId1",
                principalTable: "ticketTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
