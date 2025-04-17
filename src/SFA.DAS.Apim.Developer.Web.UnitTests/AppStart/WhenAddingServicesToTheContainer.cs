using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Queries.GetUser;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Employer;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.Provider.Shared.UI.Models;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.AppStart
{
    public class WhenAddingServicesToTheContainer
    {
        [TestCase(typeof(IGovAuthEmployerAccountService))]
        [TestCase(typeof(IApiClient))]
        [TestCase(typeof(IEmployerAccountAuthorisationHandler))]
        [TestCase(typeof(IProviderAccountAuthorisationHandler))]
        [TestCase(typeof(IExternalAccountAuthorizationHandler))]
        [TestCase(typeof(IApiDescriptionHelper))]
        [TestCase(typeof(IUserService))]
        [TestCase(typeof(ITrainingProviderAuthorizationHandler))]
        public void Then_The_Dependencies_Are_Correctly_Resolved(Type toResolve)
        {
            var serviceCollection = new ServiceCollection();
            SetupServiceCollection(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();

            var type = provider.GetService(toResolve);
            
            Assert.That(type, Is.Not.Null);
        }

        [Test]
        public void Then_Resolves_Authorization_Handlers()
        {
            var serviceCollection = new ServiceCollection();
            SetupServiceCollection(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();
            
            var type = provider.GetServices(typeof(IAuthorizationHandler)).ToList();
            
            Assert.That(type, Is.Not.Null);
            type.Count.Should().Be(8);
            type.Should().ContainSingle(c => c.GetType() == typeof(EmployerAccountAuthorizationHandler));
            type.Should().ContainSingle(c => c.GetType() == typeof(ProviderAccountAuthorizationHandler));
            type.Should().ContainSingle(c => c.GetType() == typeof(EmployerViewerAuthorizationHandler));
            type.Should().ContainSingle(c => c.GetType() == typeof(ExternalAccountAuthorizationHandler));
            type.Should().ContainSingle(c => c.GetType() == typeof(ProviderEmployerExternalAccountAuthorizationHandler));
            type.Should().ContainSingle(c => c.GetType() == typeof(AccountActiveAuthorizationHandler));
            type.Should().Contain(c => c.GetType() == typeof(TrainingProviderAllRolesAuthorizationHandler));
        }
        
        [TestCase(typeof(IValidator<RegisterCommand>), typeof(RegisterCommandValidator))]
        [TestCase(typeof(IValidator<AuthenticateUserCommand>), typeof(AuthenticateUserCommandValidator))]
        [TestCase(typeof(IValidator<GetUserQuery>), typeof(GetUserQueryValidator))]
        [TestCase(typeof(IValidator<ChangePasswordCommand>), typeof(ChangePasswordCommandValidator))]
        public void Then_Resolves_Mediator_Validators(Type validatorType, Type expectedResolvedType)
        {
            var serviceCollection = new ServiceCollection();
            SetupServiceCollection(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();
            
            var type = provider.GetService(validatorType);

            type.Should().BeOfType(expectedResolvedType);
        }
        
        private static void SetupServiceCollection(ServiceCollection serviceCollection)
        {
            var configuration = GenerateConfiguration();

            serviceCollection.AddSingleton(Mock.Of<ProviderSharedUIConfiguration>());
            serviceCollection.AddSingleton(Mock.Of<IWebHostEnvironment>());
            serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
            serviceCollection.AddConfigurationOptions(configuration, AuthenticationType.Employer);
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddServiceRegistration(new ServiceParameters(), configuration);
            serviceCollection.AddMediatRValidation();
            serviceCollection.AddEmployerAuthenticationServices();
            serviceCollection.AddProviderAuthenticationServices();
            serviceCollection.AddExternalAuthenticationServices();
            serviceCollection.AddSharedAuthenticationServices();
        }
        
        private static IConfigurationRoot GenerateConfiguration()
        {
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("EmployerApimDeveloperApi:BaseUrl", "https://test.com/"),
                    new KeyValuePair<string, string>("EmployerApimDeveloperApi:Key", "123edc"),
                    new KeyValuePair<string, string>("Environment", "test"),
                }
            };

            var provider = new MemoryConfigurationProvider(configSource);

            return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
        }
    }
}