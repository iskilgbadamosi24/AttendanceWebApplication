
using AttendanceWebApplication.Data;
using AttendanceWebApplication.Models;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;
using System.Globalization;

namespace AttendanceWebApplication.Controllers
{
    public class CompareController : Controller
    {
        // TODO: Single responsibility
        // Refactor to move the Repository and Context away from the controller

        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public CompareController(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }



        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DownloadExcel(IFormFile file1, IFormFile file2)
        {
            var extensionName1 = Path.GetExtension(file1.FileName).ToLower();
            var extensionName2 = Path.GetExtension(file2.FileName).ToLower();
            var fileName1 = Path.GetFileNameWithoutExtension(file1.FileName).Trim().ToUpper();
            var fileName2 = Path.GetFileNameWithoutExtension(file2.FileName).ToUpper();
            int nameLength = fileName1.Length;
            
            if (file1 == null || file2 == null && nameLength <= 5)
                return BadRequest("Both files are required and file names must be at least 6 characters long.");

            var courseCode = fileName1.Substring(0, 6).ToUpper();
            var attendance = await _context.AttendanceSummary.FindAsync(courseCode);
            if (attendance != null)
            {
                var summary = new AttendanceSummaryViewModel
                {
                    CourseCode = courseCode,
                    TotalInAttendance = attendance.TotalInAttendance,
                    Duplicates = attendance.Duplicates,
                    TotalMissingScore = attendance.TotalMissingScore,
                    TotalNotInAttendance = attendance.TotalNotInAttendance,
                    FresherNotInAttendance = attendance.FresherNotInAttendance,
                    DownloadLink = attendance.DownloadLink,
                    CourseCodeCount = 1,
                    TotalInAttendanceCount = attendance.TotalInAttendanceCount,
                    DuplicatesCount = attendance.DuplicatesCount,
                    TotalMissingScoreCount = attendance.TotalMissingScoreCount,
                    FresherNotInAttendanceCount = attendance.FresherNotInAttendanceCount,
                    TotalNotInAttendanceCount = attendance.TotalNotInAttendanceCount,
                    TotalStudents = attendance.TotalStudents,
                    TotalScannedCount = attendance.TotalScannedCount,
                    TotalReturningNotInAttendanceCount = attendance.TotalReturningNotInAttendanceCount,
                    TotalScannedPercentange = attendance.TotalScannedPercentange,
                    TotalNotScannedPercentage = attendance.TotalNotScannedPercentage
                };
                return View("Index", summary);
            }

            var numbers1 = ReadColumnByHeaderName(file1.OpenReadStream(), "MatricNo", extensionName1);
            var numbers2 = ReadColumnByHeaderName(file2.OpenReadStream(), "MatricNo", extensionName2);
            var numbers3 = numbers2;
            // check file name for correct IForm
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
                return BadRequest("Invalid file name. Both files contain 'Attendance'");
            }
            else if (fileName2.Contains("EXAM") && fileName1.Contains("EXAM"))
            {
                return BadRequest("Invalid file name. Both files contain 'Exam'");
            }
            else
            {
                return BadRequest("Invalid file name.");
            }
            // Find duplicates in numbers1 
            var duplicates = numbers1
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            var InAttendance = numbers1.Intersect(numbers2).ToList();
            var MissingScore = numbers1.Except(numbers2).ToList();
            var NotInAttendance = numbers2.Except(numbers1).ToList();
            // Sort the list
            InAttendance.Sort();
            MissingScore.Sort();
            NotInAttendance.Sort();

            string findFresherNotInAttendance = "2024";
            var totalStudents = MissingScore.Count + NotInAttendance.Count + InAttendance.Count;

            // Extract list of Fresher Not In Attendance with the first four digits of the student ID from NotInAttendance
            var FresherNotInAttendance = NotInAttendance
                .Where(id => id.Trim().StartsWith(findFresherNotInAttendance))
                .ToList();
            var ReturningNotInAttendance = NotInAttendance
                .Where(id => !id.Trim().StartsWith(findFresherNotInAttendance))
                .ToList();
            FresherNotInAttendance.Sort();
            ReturningNotInAttendance.Sort();

            // Save Excel to disk
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Summary");
            worksheet.Cells[1, 1].Value = "In Attendance";
            worksheet.Cells[1, 2].Value = "Duplicates In Attendance";
            worksheet.Cells[1, 3].Value = "Missing Score";
            worksheet.Cells[1, 4].Value = "Fresher Not In Attendance";
            worksheet.Cells[1, 5].Value = "Returning Not In Attendance";
            worksheet.Cells[1, 6].Value = "Total eExam Recorded";
            worksheet.Cells[1, 7].Value = "Total Scanned";
            worksheet.Cells[1, 8].Value = "Total Not Attendance";
            worksheet.Cells[1, 9].Value = "Total Fresher Not Scanned";

            worksheet.Cells[2, 6].Value = totalStudents;
            worksheet.Cells[2, 7].Value = totalStudents - NotInAttendance.Count;
            worksheet.Cells[2, 8].Value = NotInAttendance.Count;
            worksheet.Cells[2, 9].Value = FresherNotInAttendance.Count;
            worksheet.Cells[2, 10].Value = ReturningNotInAttendance.Count;

            // Apply styles
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 2].Style.Font.Bold = true;
            worksheet.Cells[1, 3].Style.Font.Bold = true;
            worksheet.Cells[1, 4].Style.Font.Bold = true;
            worksheet.Cells[1, 5].Style.Font.Bold = true;
            worksheet.Cells[1, 6].Style.Font.Bold = true;
            worksheet.Cells[1, 7].Style.Font.Bold = true;
            worksheet.Cells[1, 8].Style.Font.Bold = true;
            worksheet.Cells[1, 9].Style.Font.Bold = true;

            int maxRows = Math.Max(InAttendance.Count, Math.Max(MissingScore.Count, NotInAttendance.Count));

            for (int i = 0; i < maxRows; i++)
            {
                if (i < InAttendance.Count)
                    worksheet.Cells[i + 2, 1].Value = InAttendance[i];
                if (i < duplicates.Count)
                    worksheet.Cells[i + 2, 2].Value = duplicates[i];
                if (i < MissingScore.Count)
                    worksheet.Cells[i + 2, 3].Value = MissingScore[i];
                if (i < ReturningNotInAttendance.Count)
                    worksheet.Cells[i + 2, 4].Value = ReturningNotInAttendance[i];
                if (i < FresherNotInAttendance.Count)
                    worksheet.Cells[i + 2, 5].Value = FresherNotInAttendance[i];
            }

            var folderPath = Path.Combine(_env.WebRootPath, "downloads");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{courseCode}_Summary_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var filePath = Path.Combine(folderPath, fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, package.GetAsByteArray());
            var downloadLink = $"/downloads/{fileName}";
            var redirectLink = Url.Action("DownloadExcelResult", "Compare", new { fileSessionKey = Guid.NewGuid().ToString() });

            var model = new AttendanceSummaryViewModel
            {
                CourseCode = courseCode,
                TotalInAttendance = InAttendance,
                Duplicates = duplicates,
                TotalMissingScore = MissingScore,
                TotalNotInAttendance = NotInAttendance,
                FresherNotInAttendance = FresherNotInAttendance,
                DownloadLink = downloadLink,
                CourseCodeCount = 1,
                TotalInAttendanceCount = InAttendance.Count,
                DuplicatesCount = duplicates.Count,
                TotalMissingScoreCount = MissingScore.Count,
                FresherNotInAttendanceCount = FresherNotInAttendance.Count,
                TotalNotInAttendanceCount = NotInAttendance.Count,
                TotalStudents = InAttendance.Count + NotInAttendance.Count,
                TotalScannedCount = InAttendance.Count + duplicates.Count,
                TotalReturningNotInAttendanceCount = NotInAttendance.Count - FresherNotInAttendance.Count,
                TotalScannedPercentange = Math.Round((double)((InAttendance.Count / totalStudents) * 100),2),
                TotalNotScannedPercentage = Math.Round((double)((NotInAttendance.Count / totalStudents) * 100),2)   
            };
            _context.AttendanceSummary.Add(model);
            await _context.SaveChangesAsync();



            //TempData.Put("Index", model);

            return View("Index", model);
            //return View("Summary", model);
        }

        public IActionResult Summary()
        {
            var model = _context.AttendanceSummary.ToList();
            if (model == null)
            {
                // Handle case when TempData is empty (e.g., show message or redirect)
                return RedirectToAction("Index");
            }
            return View(model);
        }


        public IActionResult ExportAllSummaryToExcel()
        {
            var summaries = _context.AttendanceSummary.ToList();

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
                worksheet.Cells[1, 1].Style.Font.Bold = true; 
                worksheet.Cells[1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 3].Style.Font.Bold = true;
                worksheet.Cells[1, 4].Style.Font.Bold = true;
                worksheet.Cells[1, 5].Style.Font.Bold = true;
                worksheet.Cells[1, 6].Style.Font.Bold = true;
                worksheet.Cells[1, 7].Style.Font.Bold = true;
                worksheet.Cells["A1"].LoadFromArrays(data);
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"AllAttendanceSummary_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public IActionResult CourseDetail(string courseCode)
        {
            var course = _context.AttendanceSummary
                .FirstOrDefault(c => c.CourseCode == courseCode);

            if (course == null)
                return NotFound();

            return View(course);
        }






        [HttpPost]
        public async Task<IActionResult> AnalyzeFiles(IFormFile file1, IFormFile file2)
        {
            if (file1 == null || file2 == null)
            {
                TempData["Error"] = "Both files are required.";
                return RedirectToAction("Index");
            }

            var numbers1 = await ReadNumbersFromFile(file1);
            var numbers2 = await ReadNumbersFromFile(file2);
            var fileName = Path.GetFileNameWithoutExtension(file1.FileName);
            if (fileName.Length < 6)
            { 
                TempData["Error"] = "File name must be at least 6 characters long.";
                return RedirectToAction("Index");
            }
            var file1Name = fileName.Length >= 6
                        ? fileName.Substring(0, 6)
                        : fileName;

            var duplicates = numbers1.Intersect(numbers2).ToList();
            var onlyInFile1 = numbers1.Except(numbers2).ToList();
            var onlyInFile2 = numbers2.Except(numbers1).ToList();

            var model = new AttendanceSummaryViewModel
            {
                CourseCode = file1Name,
                TotalStudents = duplicates.Count + onlyInFile1.Count + onlyInFile2.Count,
                TotalInAttendance = duplicates,
                TotalMissingScore = onlyInFile1,
                TotalNotInAttendance = onlyInFile2,
                DownloadLink = string.Empty
            };

            TempData["File1Name"] = file1.FileName;
            TempData["File2Name"] = file2.FileName;

            return RedirectToAction("Index");
        }

        public IActionResult AnalysisResults()
        {
            ViewBag.File1Name = TempData["File1Name"];
            ViewBag.File2Name = TempData["File2Name"];
            ViewBag.Duplicates = TempData["Duplicates"];
            ViewBag.OnlyInFile1 = TempData["OnlyInFile1"];
            ViewBag.OnlyInFile2 = TempData["OnlyInFile2"];

            return View();
        }



        private List<string> ReadColumnByHeaderName(Stream fileStream, string columnHeader, string extension)
        {
            if (string.IsNullOrWhiteSpace(columnHeader))
                throw new ArgumentException("Column header must be provided.");

            extension = extension.ToLower();

            if (extension == ".csv")
            {
                using var reader = new StreamReader(fileStream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                using var dr = new CsvDataReader(csv);
                var table = new DataTable();
                table.Load(dr);

                if (!table.Columns.Contains(columnHeader))
                    throw new Exception($"Column '{columnHeader}' not found in the CSV file.");

                return table.AsEnumerable()
                            .Select(row => row[columnHeader]?.ToString()?.Trim())
                            .Where(value => !string.IsNullOrEmpty(value))
                            .ToList();
            }
            else if (extension == ".xlsx" || extension == ".xls")
            {
                ExcelPackage.License.SetNonCommercialPersonal("ExamAttendance"); //This will also set the Author property to the name provided in the argument.

                //using var package = new ExcelPackage(stream);

                using var package = new ExcelPackage(fileStream);
                var worksheet = package.Workbook.Worksheets[0];
                var result = new List<string>();

                var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
                int targetColumn = -1;

                foreach (var cell in headerRow)
                {
                    if (cell.Value?.ToString().Trim().Equals(columnHeader, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        targetColumn = cell.Start.Column;
                        break;
                    }
                }

                if (targetColumn == -1)
                    throw new Exception($"Column '{columnHeader}' not found in the Excel file.");

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Skip header row
                {
                    var cellValue = worksheet.Cells[row, targetColumn].Value?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        result.Add(cellValue);
                    }
                }

                return result;
            }
            else
            {
                throw new Exception("Unsupported file type.");
            }
        }




        private async Task<List<string>> ReadNumbersFromFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            if (extension == ".csv")
            {
                var numbers = new List<string>();

                using var streams = file.OpenReadStream();
                using var reader = new StreamReader(streams);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<NumberRecord>();
                foreach (var record in records)
                {
                    numbers.Add(record.Number);
                }

                return numbers;
                
            }
            else if (extension == ".xlsx" || extension == ".xls")
            {
                //OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                ExcelPackage.License.SetNonCommercialPersonal("ExamAttendance"); //This will also set the Author property to the name provided in the argument.

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];
                var numbers = new List<string>();
                for (int row = 1; worksheet.Cells[row, 1].Value != null; row++)
                {
                    numbers.Add(worksheet.Cells[row, 1].Value.ToString().Trim());
                }
                return numbers;
            }
            else
            {
                throw new Exception("Unsupported file type.");
            }
        }
    }
}
