using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.TagHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.TagHelpers
{
    public class WhenBuildingApiDescription
    {
        [Test, MoqAutoData]
        public void Then_The_Text_Is_Formatted(
            string keyName,
            string apiName,
            [Frozen] Mock<IOptions<ApimDeveloperWeb>> config,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IUrlHelperFactory> urlHelperFactory)
        {
            var input = "test data.";
            var expectedInput = "test data.";
            var mockHttpContextAccessor = new Mock<IActionContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.ActionContext).Returns(new ActionContext());
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);
            urlHelper.Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.Documentation)))).Returns("");

            var helper = new ApiDescriptionHelper(urlHelperFactory.Object, mockHttpContextAccessor.Object, config.Object);

            var actual = helper.ProcessApiDescription(input, keyName,apiName);
            actual.Should().Be(expectedInput);
        }

        [Test, MoqAutoData]
        public void Then_The_Url_Is_Processed(
            string url,
            string keyName,
            string apiName,
            string documentationBaseUrl,
            [Frozen] Mock<IOptions<ApimDeveloperWeb>> config,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IUrlHelperFactory> urlHelperFactory)
        {
            config.Object.Value.DocumentationBaseUrl = documentationBaseUrl;
            var input = "";
            var expectedInput = $" Give the API key and <a href='{url}' class='govuk-link govuk-link--no-visited-state'>this link to the API page</a> to your developer.";
            var mockHttpContextAccessor = new Mock<IActionContextAccessor>();
            urlHelper.Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName.Equals(RouteNames.Documentation)
                                                                        && c.Values.ToString() == new { apiName }.ToString() 
                                                                        && c.Host.Equals(documentationBaseUrl) 
                                                                        && c.Protocol.Equals("https"))))
                                                                    .Returns(url);
            mockHttpContextAccessor.Setup(_ => _.ActionContext).Returns(new ActionContext());
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            var helper = new ApiDescriptionHelper(urlHelperFactory.Object, mockHttpContextAccessor.Object, config.Object);

            var actual = helper.ProcessApiDescription(input, keyName,apiName);
            actual.Should().Be(expectedInput);
        }
        
        [Test, MoqAutoData]
        public void Then_The_Url_And_Text_Is_Processed(
            string url,
            string keyName,
            string apiName,
            string documentationBaseUrl,
            [Frozen] Mock<IOptions<ApimDeveloperWeb>> config,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IUrlHelperFactory> urlHelperFactory)
        {
            config.Object.Value.DocumentationBaseUrl = documentationBaseUrl;
            var input = "Test data.";
            var expectedInput = $"Test data. Give the API key and <a href='{url}' class='govuk-link govuk-link--no-visited-state'>this link to the API page</a> to your developer.";
            var mockHttpContextAccessor = new Mock<IActionContextAccessor>();
            urlHelper.Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName.Equals(RouteNames.Documentation)
                                                                        && c.Values.ToString() == new { apiName }.ToString() 
                                                                        && c.Host.Equals(documentationBaseUrl) 
                                                                        && c.Protocol.Equals("https"))))
                                                                    .Returns(url);
            mockHttpContextAccessor.Setup(_ => _.ActionContext).Returns(new ActionContext());
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            var helper = new ApiDescriptionHelper(urlHelperFactory.Object, mockHttpContextAccessor.Object, config.Object);

            var actual = helper.ProcessApiDescription(input, keyName,apiName);
            actual.Should().Be(expectedInput);
        }
        
        [Test]
        [MoqInlineAutoData("VacanciesManageOuterApi-Sandbox","Test creating an advert on Find an apprenticeship using your existing systems.")]
        [MoqInlineAutoData("VacanciesOuterApi","Display recruitment adverts from Find an apprenticeship.")]
        public void Then_The_Url_And_Text_Is_Processed_But_Url_Hidden_If_Bool_Passed(
            string keyName,
            string expectedInput,
            string url,
            string apiName,
            [Frozen] Mock<IOptions<ApimDeveloperWeb>> config,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IUrlHelperFactory> urlHelperFactory)
        {
            var input = "Test data.";
            var mockHttpContextAccessor = new Mock<IActionContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.ActionContext).Returns(new ActionContext());
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);
            var helper = new ApiDescriptionHelper(urlHelperFactory.Object, mockHttpContextAccessor.Object, config.Object);

            var actual = helper.ProcessApiDescription(input, keyName,apiName, false);
            
            urlHelper.Verify(x => x.RouteUrl(It.IsAny<UrlRouteContext>()), Times.Never);
            actual.Should().Be(expectedInput);
        }

        [Test]
        [MoqInlineAutoData("VacanciesManageOuterApi-Sandbox","Test creating an advert on Find an apprenticeship using your existing systems.")]
        [MoqInlineAutoData("VacanciesOuterApi","Display recruitment adverts from Find an apprenticeship.")]
        public void Then_If_There_Is_A_Description_Substitution_That_Is_Used(
            string keyName,
            string expectedValue,
            string url,
            string apiName,
            [Frozen] Mock<IOptions<ApimDeveloperWeb>> config,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IUrlHelperFactory> urlHelperFactory)
        {
            var input = "Test data.";
            var expectedInput = $"{expectedValue} Give the API key and <a href='{url}' class='govuk-link govuk-link--no-visited-state'>this link to the API page</a> to your developer.";
            var mockHttpContextAccessor = new Mock<IActionContextAccessor>();
            urlHelper.Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.Documentation) 
                                                                      && c.Values.ToString() == (new {apiName= apiName}).ToString()))).Returns(url);
            mockHttpContextAccessor.Setup(_ => _.ActionContext).Returns(new ActionContext());
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            var helper = new ApiDescriptionHelper(urlHelperFactory.Object, mockHttpContextAccessor.Object, config.Object);
            
            var actual = helper.ProcessApiDescription(input, keyName, apiName);
            actual.Should().Be(expectedInput);
        }
    }
}