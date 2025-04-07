using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEase_Final.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Event_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EventDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Event_ID);
                });

            migrationBuilder.CreateTable(
                name: "Venue",
                columns: table => new
                {
                    Venue_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VenueLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VenueCapacity = table.Column<int>(type: "int", nullable: true),
                    VenueImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venue", x => x.Venue_ID);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Booking_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Venue_ID = table.Column<int>(type: "int", nullable: true),
                    Event_ID = table.Column<int>(type: "int", nullable: true),
                    Venue_ID1 = table.Column<int>(type: "int", nullable: true),
                    Event_ID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Booking_ID);
                    table.ForeignKey(
                        name: "FK_Booking_Event_Event_ID1",
                        column: x => x.Event_ID1,
                        principalTable: "Event",
                        principalColumn: "Event_ID");
                    table.ForeignKey(
                        name: "FK_Booking_Venue_Venue_ID1",
                        column: x => x.Venue_ID1,
                        principalTable: "Venue",
                        principalColumn: "Venue_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Event_ID1",
                table: "Booking",
                column: "Event_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Venue_ID1",
                table: "Booking",
                column: "Venue_ID1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Venue");
        }
    }
}
