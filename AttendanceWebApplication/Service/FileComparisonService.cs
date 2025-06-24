using AttendanceWebApplication.Models;
using AttendanceWebApplication.Repository;
using OfficeOpenXml;

namespace AttendanceWebApplication.Service
{
    public class FileComparisonService : IFileComparisonService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public FileComparisonService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<AttendanceSummaryViewModel> CompareFilesAsync(IFormFile file1, IFormFile file2, string webRootPath)
        {
            // Validate files
            var extensionName1 = Path.GetExtension(file1.FileName).ToLower();
            var extensionName2 = Path.GetExtension(file2.FileName).ToLower();
            var fileName1 = Path.GetFileNameWithoutExtension(file1.FileName).Trim().ToUpper();
            var fileName2 = Path.GetFileNameWithoutExtension(file2.FileName).ToUpper();
            int nameLength = fileName1.Length;

            if (file1 == null || file2 == null && nameLength <= 5)
                throw new ArgumentException("Both files are required and file names must be at least 6 characters long.");

            var courseCode = fileName1.Substring(0, 6).ToUpper();
            var attendance = await _attendanceRepository.GetByCourseCodeAsync(courseCode);

            if (attendance != null)
            {
                return attendance;
            }

            // Process files
            var numbers1 = ReadColumnByHeaderName(file1.OpenReadStream(), "MatricNo", extensionName1);
            var numbers2 = ReadColumnByHeaderName(file2.OpenReadStream(), "MatricNo", extensionName2);
            var numbers3 = numbers2;

            // Validate file names
            if (fileName1.Contains("ATTENDANCE") && fileName2.Contains("EXAM"))
            {
                numbers1 = numbers1;
                numbers2 = numbers2;
            }
            else if (fileName1.Contains("EXAM") && fileName2.Contains("ATTENDANCE"))
            {
                numbers2 = numbers1;
                numbers1 = numbers3;
            }
            else if (fileName2.Contains("ATTENDANCE") && fileName1.Contains("ATTENDANCE"))
            {
                throw new ArgumentException("Invalid file name. Both files contain 'Attendance'");
            }
            else if (fileName2.Contains("EXAM") && fileName1.Contains("EXAM"))
            {
                throw new ArgumentException("Invalid file name. Both files contain 'Exam'");
            }
            else
            {
                throw new ArgumentException("Invalid file name.");
            }

            // Process data
            var duplicates = numbers1
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            var InAttendance = numbers1.Intersect(numbers2).ToList();
            var MissingScore = numbers1.Except(numbers2).ToList();
            var NotInAttendance = numbers2.Except(numbers1).ToList();

            InAttendance.Sort();
            MissingScore.Sort();
            NotInAttendance.Sort();

            string findFresherNotInAttendance = "2024";
            var totalStudents = MissingScore.Count + NotInAttendance.Count + InAttendance.Count;

            var FresherNotInAttendance = NotInAttendance
                .Where(id => id.Trim().StartsWith(findFresherNotInAttendance))
                .ToList();
            var ReturningNotInAttendance = NotInAttendance
                .Where(id => !id.Trim().StartsWith(findFresherNotInAttendance))
                .ToList();

            FresherNotInAttendance.Sort();
            ReturningNotInAttendance.Sort();

            // Generate Excel file
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Summary");

            var folderPath = Path.Combine(webRootPath, "downloads");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{courseCode}_Summary_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var filePath = Path.Combine(folderPath, fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, package.GetAsByteArray());
            var downloadLink = $"/downloads/{fileName}";

            // Create model
            var model = new AttendanceSummaryViewModel
            {
                CourseCode = courseCode,
                TotalInAttendance = InAttendance,
                Duplicates = duplicates,
                TotalMissingScore = MissingScore,
                TotalNotInAttendance = NotInAttendance,
                FresherNotInAttendance = FresherNotInAttendance,
                DownloadLink = downloadLink,
            };

            await _attendanceRepository.AddAsync(model);
            return model;
        }

        public List<string> ReadColumnByHeaderName(Stream fileStream, string columnHeader, string extension)
        {
            // ... (ReadColumnByHeaderName implementation away from the controller)
            return new List<string>();
        }

        public Task<List<string>> ReadNumbersFromFile(IFormFile file)
        {
            // ... (ReadNumbersFromFile implementation from the controller)
            return Task.FromResult(new List<string>());
        }

        public MemoryStream GenerateExcelForAllSummaries(List<AttendanceSummaryViewModel> summaries)
        {
            ExcelPackage.License.SetNonCommercialPersonal("ExamAttendance");

            using var package = new ExcelPackage();

            foreach (var summary in summaries)
            {
                var worksheet = package.Workbook.Worksheets.Add(summary.CourseCode ?? "Unknown");

                var data = new List<object[]>
            {
                new object[] {
                    "Course Code", "Total Students", "In Attendance", "Duplicate Students",
                    "Missing Score", "Not In Attendance", "Fresher Not In Attendance"
                },
                new object[] {
                    summary.CourseCode,
                    summary.TotalStudents,
                    summary.TotalInAttendanceCount,
                    summary.DuplicatesCount,
                    summary.TotalMissingScoreCount,
                    summary.TotalNotInAttendanceCount,
                    summary.FresherNotInAttendanceCount
                }
            };

            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
