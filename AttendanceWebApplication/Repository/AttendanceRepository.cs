using AttendanceWebApplication.Data;
using AttendanceWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceWebApplication.Repository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttendanceSummaryViewModel> GetByCourseCodeAsync(string courseCode)
        {
            return await _context.AttendanceSummary.FindAsync(courseCode);
        }

        public async Task AddAsync(AttendanceSummaryViewModel model)
        {
            _context.AttendanceSummary.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AttendanceSummaryViewModel>> GetAllAsync()
        {
            return await _context.AttendanceSummary.ToListAsync();
        }

        public async Task<AttendanceSummaryViewModel> GetFirstByCourseCodeAsync(string courseCode)
        {
            return await _context.AttendanceSummary
                .FirstOrDefaultAsync(c => c.CourseCode == courseCode);
        }
    }
}
