using System.Text.Json;
using CRA_Check.Models;

namespace CRA_Check.Tools.Parser
{
    /// <summary>
    /// Simple parser for CycloneDX format
    /// </summary>
    public static class CycloneDXParser
    {
        /// <summary>
        /// Parse components
        /// </summary>
        /// <param name="json">Components as CycloneDX format (JSON)</param>
        /// <returns>List of Component</returns>
        public static List<Component> ParseComponents(string json)
        {
            List<Component> components = new List<Component>();
            List<Vulnerability> vulnerabilities = ParseVulnerabilities(json);

            JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            if (root.TryGetProperty("components", out var componentsJson))
            {
                foreach (var componentJson in componentsJson.EnumerateArray())
                {
                    Component component = new Component();

                    component.Name = componentJson.GetProperty("name").GetString();
                    component.Version = componentJson.GetProperty("version").GetString();
                    
                    // Add vulnerabilities
                    string sbomRef = componentJson.GetProperty("bom-ref").GetString();
                    foreach (var vulnerability in vulnerabilities.Where(v => v.SbomRefs.Contains(sbomRef)))
                    {
                        component.Vulnerabilities.Add(vulnerability);
                        vulnerability.Components.Add(component);
                    }

                    components.Add(component);
                }
            }

            return components;
        }

        /// <summary>
        /// Parse vulnerabilities
        /// </summary>
        /// <param name="json">Vulnerabilities as CycloneDX format (JSON)</param>
        /// <returns>List of Vulnerability</returns>
        public static List<Vulnerability> ParseVulnerabilities(string json)
        {
            List<Vulnerability> vulnerabilities = new List<Vulnerability>();

            JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            if (root.TryGetProperty("vulnerabilities", out var vulnerabilitiesJson))
            {
                foreach (var vulnerabilityJson in vulnerabilitiesJson.EnumerateArray())
                {
                    Vulnerability vulnerability = new Vulnerability();

                    vulnerability.VulnerabilityId = vulnerabilityJson.GetProperty("id").GetString();
                    vulnerability.Description = vulnerabilityJson.GetProperty("description").GetString();
                    vulnerability.SourceName = vulnerabilityJson.GetProperty("source").GetProperty("name").GetString();
                    vulnerability.SourceUrl = vulnerabilityJson.GetProperty("source").GetProperty("url").GetString();

                    foreach (var ratingJson in vulnerabilityJson.GetProperty("ratings").EnumerateArray())
                    {
                        var rating = new Rating();

                        if (ratingJson.TryGetProperty("score", out var scoreJson))
                        {
                            rating.Score = scoreJson.GetDouble();
                        }

                        string severityStr = ratingJson.GetProperty("severity").GetString();
                        rating.Severity = Enum.Parse<SeverityLevel>(severityStr);

                        rating.Method = ratingJson.GetProperty("method").GetString();

                        if (ratingJson.TryGetProperty("vector", out var vectorJson))
                        {
                            rating.Vector = vectorJson.GetString();
                        }

                        vulnerability.Ratings.Add(rating);
                    }

                    foreach (var affectsJson in vulnerabilityJson.GetProperty("affects").EnumerateArray())
                    {
                        vulnerability.SbomRefs.Add(affectsJson.GetProperty("ref").GetString());
                    }

                    vulnerabilities.Add(vulnerability);
                }
            }

            return vulnerabilities;
        }
    }
}
