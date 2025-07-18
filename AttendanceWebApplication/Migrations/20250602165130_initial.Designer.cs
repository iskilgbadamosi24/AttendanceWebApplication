﻿// <auto-generated />
using System;
using AttendanceWebApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AttendanceWebApplication.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250602165130_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AttendanceWebApplication.Models.AttendanceDto", b =>
                {
                    b.Property<string>("CourseCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AttendanceDate")
                        .HasColumnType("datetime2");

                    b.PrimitiveCollection<string>("Duplicates")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalInAttendanceCount")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("UniqueAttendanceMatricNos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseCode");

                    b.ToTable("AtendanceDtos");
                });

            modelBuilder.Entity("AttendanceWebApplication.Models.AttendanceRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AttendanceDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CollegeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeptName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatricNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ScanDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("AttendanceRecords");
                });

            modelBuilder.Entity("AttendanceWebApplication.Models.AttendanceSummaryViewModel", b =>
                {
                    b.Property<string>("CourseCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CourseCodeCount")
                        .HasColumnType("int");

                    b.Property<string>("DownloadLink")
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("Duplicates")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DuplicatesCount")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("FresherNotInAttendance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FresherNotInAttendanceCount")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("TotalInAttendance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TotalInAttendanceCount")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("TotalMissingScore")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TotalMissingScoreCount")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("TotalNotInAttendance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TotalNotInAttendanceCount")
                        .HasColumnType("int");

                    b.Property<int?>("TotalStudents")
                        .HasColumnType("int");

                    b.HasKey("CourseCode");

                    b.ToTable("AttendanceSummary");
                });

            modelBuilder.Entity("AttendanceWebApplication.Models.ExamListDto", b =>
                {
                    b.Property<string>("CoureseCode")
                        .HasColumnType("nvarchar(450)");

                    b.PrimitiveCollection<string>("ExamMatricNos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CoureseCode");

                    b.ToTable("ExamListDtos");
                });
#pragma warning restore 612, 618
        }
    }
}
