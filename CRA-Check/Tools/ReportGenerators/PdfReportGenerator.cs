
using CRA_Check.Models;
using QuestPDF.Fluent;

namespace CRA_Check.Tools.ReportGenerators
{
    public class PdfReportGenerator : IReportGenerator
    {
        public void GenerateReport(Release release, string destinationFilename)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text($"Vulnerabilies report - {release.Software.Name} ({release.VersionStr})").FontSize(20).SemiBold();
                    page.Content().PaddingVertical(10).Column(componentsColumn =>
                    {
                        foreach (var component in release.Components.OrderByDescending(c => c.MaxVulnerabilityRating))
                        {
                            componentsColumn.Item().Text($"Component : {component.Name} ({component.Version})");
                            foreach (var vulnerability in component.Vulnerabilities.OrderByDescending(v => v.MaxRating))
                            {
                                componentsColumn.Item().PaddingHorizontal(30).Row(vulnerabilitiesRow =>
                                {
                                    vulnerabilitiesRow.RelativeItem().Text($"ID: {vulnerability.VulnerabilityId}");
                                    vulnerabilitiesRow.RelativeItem().Text($"Score: {vulnerability.Ratings[0].Score}");
                                    vulnerabilitiesRow.RelativeItem().Text($"Severity: {vulnerability.Ratings[0].Severity}");
                                });
                                //page.Content().PaddingVertical(10).PaddingHorizontal(30).Column(vulnerabilitiesColumn =>
                                //{
                                //    vulnerabilitiesColumn.Item().Text("Test");
                                //});
                            }
                        }

                        componentsColumn.Item().PaddingBottom(10);
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
