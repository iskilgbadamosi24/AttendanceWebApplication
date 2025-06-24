namespace AttendanceWebApplication.Models
{
    public class SummaryViewDto
    {
        public string CourseCode { get; set; }
        public string TotalStudents { get; set; }
        public string TotalInAttendance { get; set; }
        public string TotalMissingScore { get; set; }
        public string TotalNotInAttendance { get; set; }
        public string DownloadLink { get; set; }
    }
}

