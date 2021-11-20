using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure.Configuration;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Errors
{
    public class WhenViewingTheAccessDeniedPage
    {
        [Test, MoqAutoData]
        public async Task Then_The_HomePage_Url_Is_Passed_To_The_View(
            string homepageUrl,
            [Frozen] Mock<IOptions<ExternalLinksConfiguration>> externalLinks,
            [Greedy] ErrorController errorController)
        {
            externalLinks.Object.Value.ManageApprenticeshipSiteUrl = homepageUrl;
            
            var actual = errorController.AccessDenied() as ViewResult;

            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("AccessDenied");
            var actualModel = actual.Model as string;
            Assert.IsNotNull(actualModel);
            actualModel.Should().Be(homepageUrl);
        }
    }
}