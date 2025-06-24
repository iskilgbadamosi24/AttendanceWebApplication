using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AttendanceWebApplication.Models
{
    public class AttendanceRecord
    {
       
        [Name("attendance_date")]
        public DateTime AttendanceDate { get; set; }

        [Name("scan_datetime")]
        public DateTime ScanDateTime { get; set; }

        [Name("LastName")]
        public string LastName { get; set; }

        [Name("FirstName")]
        public string FirstName { get; set; }

        [Name("PhotoURL")]
        public string PhotoUrl { get; set; }

        [Name("MatricNo")]
        public string MatricNo { get; set; }

        [Name("Level")]
        public string Level { get; set; }

        [Name("Faculty")]
        public string CollegeName { get; set; }

        [Name("Department")]
        public string DeptName { get; set; }
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

    }
}
