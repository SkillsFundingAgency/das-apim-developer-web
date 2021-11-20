using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.TagHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.TagHelpers
{
    public class WhenBuildingApiDescription
    {
        [Test, MoqAutoData]
        public void Then_The_Text_Is_Formatted(
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IUrlHelperFactory> urlHelperFactory)
        {
            var input = "test data.";
            var expectedInput = "test data.<br>";
            var mockHttpContextAccessor = new Mock<IActionContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.ActionContext).Returns(new ActionContext());
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            var hlper = new ApiDescriptionHelper(urlHelperFactory.Object, mockHttpContextAccessor.Object);

            var actual = hlper.ProcessApiDescription(input);
            actual.Should().Be(expectedInput);
        }

        [Test, MoqAutoData]
        public void Then_The_Url_Is_Processed(
            string url,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IUrlHelperFactory> urlHelperFactory)
        {
            var input = "{{test data|test_data}}";
            var expectedInput = $"<a href='{url}' class='govuk-link govuk-link--no-visited-state'>test data</a>";
            var mockHttpContextAccessor = new Mock<IActionContextAccessor>();
            urlHelper.Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals("test_data")))).Returns(url);
            mockHttpContextAccessor.Setup(_ => _.ActionContext).Returns(new ActionContext());
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            var hlper = new ApiDescriptionHelper(urlHelperFactory.Object, mockHttpContextAccessor.Object);

            var actual = hlper.ProcessApiDescription(input);
            actual.Should().Be(expectedInput);
        }
        
        [Test, MoqAutoData]
        public void Then_The_Url_And_Text_Is_Processed(
            string url,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IUrlHelperFactory> urlHelperFactory)
        {
            var input = "Test data. This is {{test data|test_data}}";
            var expectedInput = $"Test data.<br> This is <a href='{url}' class='govuk-link govuk-link--no-visited-state'>test data</a>";
            var mockHttpContextAccessor = new Mock<IActionContextAccessor>();
            urlHelper.Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals("test_data")))).Returns(url);
            mockHttpContextAccessor.Setup(_ => _.ActionContext).Returns(new ActionContext());
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            var hlper = new ApiDescriptionHelper(urlHelperFactory.Object, mockHttpContextAccessor.Object);

            var actual = hlper.ProcessApiDescription(input);
            actual.Should().Be(expectedInput);
        }
    }
}