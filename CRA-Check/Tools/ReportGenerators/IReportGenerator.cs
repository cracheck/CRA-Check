using CRA_Check.Models;

namespace CRA_Check.Tools.ReportGenerators
{
    public interface IReportGenerator
    {
        public void GenerateReport(IList<Component> components, string destinationPath);

        public void GenerateReport(string sourcePath, string destinationPath);
    }
}
