using SyftCLIWrapper;

namespace CRA_Check.Tools.SbomGenerators
{
    public class SyftSbomGenerator : ISbomGenerator
    {
        private SyftCLIWrapper.SyftCLIWrapper _syftCLIWrapper;

        public SyftSbomGenerator(string syftPath)
        {
            _syftCLIWrapper = new SyftCLIWrapper.SyftCLIWrapper(syftPath);
        }

        public Task<string> GenerateSbom(string sourcePath)
        {
            return _syftCLIWrapper.GenerateSBOM(sourcePath, OutputFormatType.CYCLONEDX_JSON);
        }

        public Task<Version> GetVersion()
        {
            return _syftCLIWrapper.GetVersion();
        }
    }
}
