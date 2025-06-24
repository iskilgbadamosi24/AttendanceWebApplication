using AttendanceWebApplication.Models;

namespace AttendanceWebApplication.Service
{
    public interface IFileComparisonService
    {
        Task<AttendanceSummaryViewModel> CompareFilesAsync(IFormFile file1, IFormFile file2, string webRootPath);
        List<string> ReadColumnByHeaderName(Stream fileStream, string columnHeader, string extension);
        Task<List<string>> ReadNumbersFromFile(IFormFile file);
        MemoryStream GenerateExcelForAllSummaries(List<AttendanceSummaryViewModel> summaries);
    }
}
