using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Home
{
    public class WhenGettingTheDashboard
    {
        [Test, MoqAutoData]
        public void Then_The_Redirect(
            string dashboardUrl,
            [Frozen] ProviderSharedUIConfiguration config,
            [Greedy] HomeController controller)
        {
            config.DashboardUrl = dashboardUrl;

            var actual = controller.Dashboard() as RedirectResult;
            
            Assert.IsNotNull(actual);
            actual.Url.Should().Be(dashboardUrl);
        }
    }
}