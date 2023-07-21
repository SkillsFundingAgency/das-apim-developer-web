namespace SFA.DAS.Apim.Developer.Domain.Configuration
{
    public class ApimDeveloperWeb
    {
        public string DataProtectionKeysDatabase { get ; set ; }
        public string RedisConnectionString { get ; set ; }
        public string DocumentationBaseUrl { get; set; }
        public bool UseGovSignIn { get; set; }
        public bool UseDfESignIn { get; set; }
    }
}