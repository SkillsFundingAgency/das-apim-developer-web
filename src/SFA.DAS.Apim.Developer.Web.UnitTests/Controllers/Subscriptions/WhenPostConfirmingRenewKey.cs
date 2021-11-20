using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Subscriptions
{
    public class WhenPostConfirmingRenewKey
    {
        [Test, MoqAutoData]
        public void Then_If_The_Model_Is_Not_Valid_Then_Redirect_To_Confirm(
            string employerAccountId,
            string id,
            RenewKeyViewModel viewModel,
            [Greedy]SubscriptionsController controller)
        {
            controller.ModelState.AddModelError("key", "error message");

            var actual = controller.PostConfirmRenewKey(employerAccountId, id, viewModel) as ViewResult;

            actual.ViewName.Should().Be("ConfirmRenewKey");
            actual.Model.Should().BeEquivalentTo(viewModel);
        }
        
        [Test, MoqAutoData]
        public void Then_If_The_Model_Is_Valid_And_Not_Renewing_Key_Redirect_To_Hub(
            string employerAccountId,
            string id,
            RenewKeyViewModel viewModel,
            [Greedy]SubscriptionsController controller)
        {
            viewModel.ConfirmRenew = false;

            var actual = controller.PostConfirmRenewKey(employerAccountId, id, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.EmployerApiHub);
            actual.RouteValues.Should().ContainKey("employerAccountId");
            actual.RouteValues["employerAccountId"].Should().Be(employerAccountId);
        }
        
        [Test, MoqAutoData]
        public void Then_If_The_Model_Is_Valid_And_Renewing_Key_Redirect_To_Key_Renewed(
            string employerAccountId,
            string id,
            RenewKeyViewModel viewModel,
            [Greedy]SubscriptionsController controller)
        {
            viewModel.ConfirmRenew = true;

            var actual = controller.PostConfirmRenewKey(employerAccountId, id, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.EmployerKeyRenewed);
            actual.RouteValues.Should().ContainKey("employerAccountId");
            actual.RouteValues["employerAccountId"].Should().Be(employerAccountId);
        }
    }
}