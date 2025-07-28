using CRA_Check.Models;

namespace CRA_Check.Tools.ReportGenerators
{
    public interface IReportGenerator
    {
        public void GenerateReport(Release release, string destinationPath);

        public void GenerateReport(string sourcePath, string destinationPath);
    }
}
