using CRA_Check.Models;

namespace CRA_Check.Tools.ReportGenerators
{
    public interface IReportGenerator
    {
        public void GenerateReport(IList<Vulnerability> vulnerabilities, string destinationPath);

        public void GenerateReport(string sourcePath, string destinationPath);
    }
}
