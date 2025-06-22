using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEase_Final.Migrations
{
    /// <inheritdoc />
    public partial class BookingDateRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "Booking",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Booking",
                newName: "BookingDate");
        }
    }
}
