using System.ComponentModel.DataAnnotations;

namespace AttendanceWebApplication.Models
{
    public class AttendanceDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Key]
        public string CourseCode { get; set; }
        public DateTime AttendanceDate { get; set; }
        public List<string> UniqueAttendanceMatricNos { get; set; }
        public List<string> Duplicates { get; set; }
        public int TotalInAttendanceCount { get; set; }
    }
}
