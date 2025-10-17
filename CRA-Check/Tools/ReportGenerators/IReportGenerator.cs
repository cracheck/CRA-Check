using CRA_Check.Models;

namespace CRA_Check.Tools.ReportGenerators
{
    /// <summary>
    /// Interface of vulnerabilities report generator
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Generate a vulnerabilities report for a release
        /// </summary>
        /// <param name="release">Release</param>
        /// <param name="destinationPath">Output file for the report</param>
        public void GenerateReport(Release release, string destinationPath);
    }
}
