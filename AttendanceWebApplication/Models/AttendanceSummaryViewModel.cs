using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AttendanceWebApplication.Models
{
    public class AttendanceSummaryViewModel
    {
        [Key]
        public string CourseCode { get; set; }
        public List<string>? TotalInAttendance { get; set; }
        public List<string>? Duplicates { get; set; }
        public List<string>? TotalMissingScore { get; set; }
        public List<string>? TotalNotInAttendance { get; set; }
        public List<string>? FresherNotInAttendance { get; set; }
        public string? DownloadLink { get; set; }
        public int? DuplicatesCount { get; set; }
        public int? TotalInAttendanceCount { get; set; }
        public int? TotalMissingScoreCount { get; set; }
        public int? TotalNotInAttendanceCount { get; set; }
        public int? TotalStudents { get; set; } 
        public int? CourseCodeCount { get; set; }
        public int? FresherNotInAttendanceCount { get; set; }
        public int? TotalReturningNotInAttendanceCount { get; set; }
        public int? TotalScannedCount { get; set; }
        public double? TotalScannedPercentange { get; set; }
        public double? TotalNotScannedPercentage { get; set; }

        
    }
}
