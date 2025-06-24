using AttendanceWebApplication.Models;

namespace AttendanceWebApplication.Repository
{
    public interface IAttendanceRepository
    {
        Task<AttendanceSummaryViewModel> GetByCourseCodeAsync(string courseCode);
        Task AddAsync(AttendanceSummaryViewModel model);
        Task<List<AttendanceSummaryViewModel>> GetAllAsync();
        Task<AttendanceSummaryViewModel> GetFirstByCourseCodeAsync(string courseCode);
    }
}
