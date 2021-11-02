using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.AppStart
{
    public class WhenAddingServicesToTheContainer
    {
        [TestCase(typeof(IEmployerAccountService))]
        [TestCase(typeof(IApiClient))]
        [TestCase(typeof(IEmployerAccountAuthorisationHandler))]
        public void Then_The_Dependencies_Are_Correctly_Resolved(Type toResolve)
        {
            var hostEnvironment = new Mock<IWebHostEnvironment>();
            var serviceCollection = new ServiceCollection();

            var configuration = GenerateConfiguration();
            
            serviceCollection.AddSingleton(hostEnvironment.Object);
            serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
            serviceCollection.AddConfigurationOptions(configuration);
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddServiceRegistration(new ServiceParameters(),configuration);
            serviceCollection.AddEmployerAuthenticationServices();

            var provider = serviceCollection.BuildServiceProvider();

            var type = provider.GetService(toResolve);
            Assert.IsNotNull(type);
        }
        private static IConfigurationRoot GenerateConfiguration()
        {
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("ApimDeveloperApi:BaseUrl", "https://test.com/"),
                    new KeyValuePair<string, string>("ApimDeveloperApi:Key", "123edc"),
                    new KeyValuePair<string, string>("Environment", "test"),
                }
            };

            var provider = new MemoryConfigurationProvider(configSource);

            return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
        }
    }
}