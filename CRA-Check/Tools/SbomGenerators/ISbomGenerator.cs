namespace CRA_Check.Tools.SbomGenerators
{
    /// <summary>
    /// Interface for SBOM generator
    /// </summary>
    public interface ISbomGenerator
    {
        /// <summary>
        /// Generate SBOM from a path
        /// </summary>
        /// <param name="sourcePath">Source path</param>
        /// <returns>SBOM</returns>
        public Task<string> GenerateSbom(string sourcePath);

        /// <summary>
        /// Return the version of the SBOM generator
        /// </summary>
        /// <returns>Version</returns>
        public Task<Version> GetVersion();
    }
}
