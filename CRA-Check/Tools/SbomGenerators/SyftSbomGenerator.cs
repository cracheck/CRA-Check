using SyftCLIWrapper;

namespace CRA_Check.Tools.SbomGenerators
{
    /// <summary>
    /// Syft SBOM generator
    /// </summary>
    public class SyftSbomGenerator : ISbomGenerator
    {
        /// <summary>
        /// Syft wrapper
        /// </summary>
        private SyftCLIWrapper.SyftCLIWrapper _syftCLIWrapper;

        /// <summary>
        /// Constructor. Configure the Syft wrapper
        /// </summary>
        /// <param name="syftPath">Path to Syft.exe</param>
        public SyftSbomGenerator(string syftPath)
        {
            _syftCLIWrapper = new SyftCLIWrapper.SyftCLIWrapper(syftPath);
        }

        /// <summary>
        /// Generate SBOM from a path
        /// </summary>
        /// <param name="sourcePath">Source path</param>
        /// <returns>SBOM</returns>
        public Task<string> GenerateSbom(string sourcePath)
        {
            return _syftCLIWrapper.GenerateSBOM(sourcePath, OutputFormatType.CYCLONEDX_JSON);
        }

        /// <summary>
        /// Return the version of the SBOM generator
        /// </summary>
        /// <returns>Version</returns>
        public Task<Version> GetVersion()
        {
            return _syftCLIWrapper.GetVersion();
        }
    }
}
