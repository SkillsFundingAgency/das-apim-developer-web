using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace SFA.DAS.Apim.Developer.Web.AcceptanceTests.Infrastructure
{
    public static class ConfigBuilder
    {
        public static IConfigurationRoot GenerateConfiguration(string authType)
        {
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new[]
                {
                    new KeyValuePair<string, string>("ConfigurationStorageConnectionString", "UseDevelopmentStorage=true;"),
                    new KeyValuePair<string, string>("ConfigNames", "SFA.DAS.Apim.Developer.Web"),
                    new KeyValuePair<string, string>("Environment", "DEV"),
                    new KeyValuePair<string, string>("Version", "1.0"),
                    new KeyValuePair<string, string>("StubAuth", "true"),
                    new KeyValuePair<string, string>("AuthType", authType),
                    new KeyValuePair<string, string>("ResourceEnvironmentName", "test"),

                    new KeyValuePair<string, string>($"{authType}ApimDeveloperApi:Key", "test"),
                    new KeyValuePair<string, string>($"{authType}ApimDeveloperApi:BaseUrl", "http://localhost:5031/"),
                    new KeyValuePair<string, string>("ProviderIdams:MetadataAddress", ""),
                    new KeyValuePair<string, string>("ProviderIdams:Wtrealm", "https://localhost:5011/"),
                    new KeyValuePair<string, string>("ProviderSharedUIConfiguration:DashboardUrl", "https://at-pas.apprenticeships.education.gov.uk/"),
                    
                }
            };

            var provider = new MemoryConfigurationProvider(configSource);

            return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
        }
    }
}