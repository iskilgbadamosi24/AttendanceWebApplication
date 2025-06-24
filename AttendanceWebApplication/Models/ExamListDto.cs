using System.ComponentModel.DataAnnotations;

namespace AttendanceWebApplication.Models
{
    public class ExamListDto
    {
        [Key]
        public string CoureseCode { get; set; }
        public List<string> ExamMatricNos { get; set; }
       
    }
}
