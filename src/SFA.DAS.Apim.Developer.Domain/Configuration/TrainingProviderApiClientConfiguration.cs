namespace SFA.DAS.Apim.Developer.Domain.Configuration
{
    public record TrainingProviderApiClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set; }
        public string Version { get; set; } = "1.0";
    }
}
