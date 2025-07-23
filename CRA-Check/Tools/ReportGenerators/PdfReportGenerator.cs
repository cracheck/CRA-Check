
using CRA_Check.Models;
using QuestPDF.Fluent;

namespace CRA_Check.Tools.ReportGenerators
{
    public class PdfReportGenerator : IReportGenerator
    {
        public void GenerateReport(IList<Vulnerability> vulnerabilities, string destinationFilename)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Vulnerabilies report").FontSize(20).SemiBold();
                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        foreach (var vulnerability in vulnerabilities)
                        {
                            column.Item().Text($"Component : {vulnerability.SourceName} ({vulnerability.Id})");
                        }

                        column.Item().PaddingBottom(10);
                    });

                    page.Footer().AlignCenter().Text("asd").FontSize(10);
                });
            }).GeneratePdf(destinationFilename);
        }

        public void GenerateReport(string sourcePath, string destinationPath)
        {

        }
    }
}
