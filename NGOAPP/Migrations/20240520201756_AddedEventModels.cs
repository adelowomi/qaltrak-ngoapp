using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NGOAPP.Migrations
{
    /// <inheritdoc />
    public partial class AddedEventModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: true),
                    latitude = table.Column<double>(type: "double", nullable: true),
                    images = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    eventContact = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    totalCapacity = table.Column<int>(type: "int", nullable: true),
                    numberOfVolunteersNeeded = table.Column<int>(type: "int", nullable: true),
                    attendeesCanVolunteer = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    questionsForAttendees = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tags = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventTypeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    eventCategoryId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    eventSubCategoryId = table.Column<int>(type: "int", nullable: true),
                    publishDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    isPrivate = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    isPublished = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    dateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_events", x => x.id);
                    table.ForeignKey(
                        name: "fK_events_eventCategories_eventCategoryId",
                        column: x => x.eventCategoryId,
                        principalTable: "eventCategories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fK_events_eventSubCategories_eventSubCategoryId",
                        column: x => x.eventSubCategoryId,
                        principalTable: "eventSubCategories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fK_events_eventTypes_eventTypeId",
                        column: x => x.eventTypeId,
                        principalTable: "eventTypes",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "speakers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bio = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventId = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_speakers", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    department = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    altPhoneNumber1 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    altPhoneNumber2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    dateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_contacts", x => x.id);
                    table.ForeignKey(
                        name: "fK_contacts_events_eventId",
                        column: x => x.eventId,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_contacts_users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "eventTickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    eventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ticketTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    maxQuantityPerOrder = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    tax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    isSoldOut = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ticketTypeId1 = table.Column<int>(type: "int", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_eventTickets", x => x.id);
                    table.ForeignKey(
                        name: "fK_eventTickets_events_eventId",
                        column: x => x.eventId,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_eventTickets_ticketTypes_ticketTypeId1",
                        column: x => x.ticketTypeId1,
                        principalTable: "ticketTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    addressLine = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    locationUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tags = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    locationTypeId = table.Column<int>(type: "int", nullable: true),
                    eventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    dateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_locations", x => x.id);
                    table.ForeignKey(
                        name: "fK_locations_events_eventId",
                        column: x => x.eventId,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_locations_locationTypes_locationTypeId",
                        column: x => x.locationTypeId,
                        principalTable: "locationTypes",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    dateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_schedules", x => x.id);
                    table.ForeignKey(
                        name: "fK_schedules_events_eventId",
                        column: x => x.eventId,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    eventId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    eventTicketId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    free = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ticketTypeId = table.Column<int>(type: "int", nullable: true),
                    dateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_tickets", x => x.id);
                    table.ForeignKey(
                        name: "fK_tickets_eventTickets_eventTicketId",
                        column: x => x.eventTicketId,
                        principalTable: "eventTickets",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fK_tickets_events_eventId",
                        column: x => x.eventId,
                        principalTable: "events",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fK_tickets_ticketTypes_ticketTypeId",
                        column: x => x.ticketTypeId,
                        principalTable: "ticketTypes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fK_tickets_users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "medias",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    locationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    speakerId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    dateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_medias", x => x.id);
                    table.ForeignKey(
                        name: "fK_medias_locations_locationId",
                        column: x => x.locationId,
                        principalTable: "locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fK_medias_speakers_speakerId",
                        column: x => x.speakerId,
                        principalTable: "speakers",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    topic = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    durationTime = table.Column<int>(type: "int", nullable: false),
                    scheduleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    eventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    locationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    youtubeLiveLink = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    youtubeLink = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    end = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fK_sessions_events_eventId",
                        column: x => x.eventId,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_sessions_schedules_scheduleId",
                        column: x => x.scheduleId,
                        principalTable: "schedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sessionSpeaker",
                columns: table => new
                {
                    sessionsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    speakersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_sessionSpeaker", x => new { x.sessionsId, x.speakersId });
                    table.ForeignKey(
                        name: "fK_sessionSpeaker_sessions_sessionsId",
                        column: x => x.sessionsId,
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_sessionSpeaker_speakers_speakersId",
                        column: x => x.speakersId,
                        principalTable: "speakers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "iX_contacts_eventId",
                table: "contacts",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "iX_contacts_userId",
                table: "contacts",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "iX_eventTickets_eventId",
                table: "eventTickets",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "iX_eventTickets_ticketTypeId1",
                table: "eventTickets",
                column: "ticketTypeId1");

            migrationBuilder.CreateIndex(
                name: "iX_events_eventCategoryId",
                table: "events",
                column: "eventCategoryId");

            migrationBuilder.CreateIndex(
                name: "iX_events_eventSubCategoryId",
                table: "events",
                column: "eventSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "iX_events_eventTypeId",
                table: "events",
                column: "eventTypeId");

            migrationBuilder.CreateIndex(
                name: "iX_locations_eventId",
                table: "locations",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "iX_locations_locationTypeId",
                table: "locations",
                column: "locationTypeId");

            migrationBuilder.CreateIndex(
                name: "iX_medias_locationId",
                table: "medias",
                column: "locationId");

            migrationBuilder.CreateIndex(
                name: "iX_medias_speakerId",
                table: "medias",
                column: "speakerId");

            migrationBuilder.CreateIndex(
                name: "iX_schedules_eventId",
                table: "schedules",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "iX_sessionSpeaker_speakersId",
                table: "sessionSpeaker",
                column: "speakersId");

            migrationBuilder.CreateIndex(
                name: "iX_sessions_eventId",
                table: "sessions",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "iX_sessions_scheduleId",
                table: "sessions",
                column: "scheduleId");

            migrationBuilder.CreateIndex(
                name: "iX_tickets_eventId",
                table: "tickets",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "iX_tickets_eventTicketId",
                table: "tickets",
                column: "eventTicketId");

            migrationBuilder.CreateIndex(
                name: "iX_tickets_ticketTypeId",
                table: "tickets",
                column: "ticketTypeId");

            migrationBuilder.CreateIndex(
                name: "iX_tickets_userId",
                table: "tickets",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "medias");

            migrationBuilder.DropTable(
                name: "sessionSpeaker");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "speakers");

            migrationBuilder.DropTable(
                name: "eventTickets");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "events");
        }
    }
}
