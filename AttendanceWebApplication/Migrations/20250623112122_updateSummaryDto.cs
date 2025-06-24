using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class updateSummaryDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalNotScannedPercentage",
                table: "AttendanceSummary",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalReturningNotInAttendanceCount",
                table: "AttendanceSummary",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalScannedCount",
                table: "AttendanceSummary",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalScannedPercentange",
                table: "AttendanceSummary",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalNotScannedPercentage",
                table: "AttendanceSummary");

            migrationBuilder.DropColumn(
                name: "TotalReturningNotInAttendanceCount",
                table: "AttendanceSummary");

            migrationBuilder.DropColumn(
                name: "TotalScannedCount",
                table: "AttendanceSummary");

            migrationBuilder.DropColumn(
                name: "TotalScannedPercentange",
                table: "AttendanceSummary");
        }
    }
}
