using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AtendanceDtos",
                columns: table => new
                {
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UniqueAttendanceMatricNos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duplicates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalInAttendanceCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtendanceDtos", x => x.CourseCode);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScanDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatricNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollegeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeptName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceSummary",
                columns: table => new
                {
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalInAttendance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duplicates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMissingScore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalNotInAttendance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FresherNotInAttendance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuplicatesCount = table.Column<int>(type: "int", nullable: true),
                    TotalInAttendanceCount = table.Column<int>(type: "int", nullable: true),
                    TotalMissingScoreCount = table.Column<int>(type: "int", nullable: true),
                    TotalNotInAttendanceCount = table.Column<int>(type: "int", nullable: true),
                    TotalStudents = table.Column<int>(type: "int", nullable: true),
                    CourseCodeCount = table.Column<int>(type: "int", nullable: true),
                    FresherNotInAttendanceCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceSummary", x => x.CourseCode);
                });

            migrationBuilder.CreateTable(
                name: "ExamListDtos",
                columns: table => new
                {
                    CoureseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExamMatricNos = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamListDtos", x => x.CoureseCode);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtendanceDtos");

            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "AttendanceSummary");

            migrationBuilder.DropTable(
                name: "ExamListDtos");
        }
    }
}
