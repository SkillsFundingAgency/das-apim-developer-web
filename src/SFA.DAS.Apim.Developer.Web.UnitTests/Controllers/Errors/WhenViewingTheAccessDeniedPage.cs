using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure.Configuration;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Errors
{
    public class WhenViewingTheAccessDeniedPage
    {
        [Test, MoqAutoData]
        public void Then_The_HomePage_Url_Is_Passed_To_The_View(
            string homepageUrl,
            bool useDfESignIn,
            [Frozen] Mock<IOptions<ExternalLinksConfiguration>> externalLinks,
            [Frozen] Mock<ApimDeveloperWeb> apimDeveloperWeb,
            [Frozen] Mock<IConfiguration> configuration,
            [Greedy] ErrorController errorController)
        {
            configuration?.SetupGet(x => x[It.Is<string>(s => s == "ResourceEnvironmentName")]).Returns("LOCAL");
            externalLinks.Object.Value.ManageApprenticeshipSiteUrl = homepageUrl;
            
            var actual = errorController.AccessDenied() as ViewResult;

            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("AccessDenied");
            var actualModel = actual.Model as Error403ViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.DashboardUrl.Should().Be(homepageUrl);
        }
    }
}