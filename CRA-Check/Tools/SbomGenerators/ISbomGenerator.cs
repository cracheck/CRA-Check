namespace CRA_Check.Tools.SbomGenerators
{
    public interface ISbomGenerator
    {
        public Task<string> GenerateSbom(string sourcePath);

        public Task<Version> GetVersion();
    }
}
