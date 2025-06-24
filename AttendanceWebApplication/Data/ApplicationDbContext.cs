using AttendanceWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceWebApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<AttendanceSummaryViewModel> AttendanceSummary { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<AttendanceDto> AtendanceDtos { get; set; } 
        public DbSet<ExamListDto> ExamListDtos { get; set; } 
    }
}
