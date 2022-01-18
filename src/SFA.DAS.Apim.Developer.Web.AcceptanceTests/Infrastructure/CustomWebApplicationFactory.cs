using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.Apim.Developer.Web.AcceptanceTests.Infrastructure
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        private readonly string _applicationType;

        public CustomWebApplicationFactory (string authType)
        {
            _applicationType = authType;
        }
         
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
                configurationBuilder.AddConfiguration(ConfigBuilder.GenerateConfiguration(_applicationType)));
        }
    }
}