using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Subscriptions
{
    public class WhenPostConfirmingRenewKey
    {
        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Not_Valid_Then_Redirect_To_Confirm(
            string employerAccountId,
            string id,
            RenewKeyViewModel viewModel,
            [Greedy]SubscriptionsController controller)
        {
            controller.ModelState.AddModelError("key", "error message");

            var actual = await controller.PostConfirmRenewKey(employerAccountId, id,null,  viewModel) as ViewResult;

            actual.ViewName.Should().Be("ConfirmRenewKey");
            actual.Model.Should().BeAssignableTo<bool>();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Not_Renewing_Key_Redirect_To_Hub_Employer(
            string employerAccountId,
            string id,
            RenewKeyViewModel viewModel,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            viewModel.ConfirmRenew = false;
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            var controller = new SubscriptionsController(Mock.Of<IMediator>(), serviceParameters.Object);
            
            var actual = await controller.PostConfirmRenewKey(employerAccountId, id, null, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.EmployerViewSubscription);
            actual.RouteValues.Should().ContainKey("employerAccountId");
            actual.RouteValues["employerAccountId"].Should().Be(employerAccountId);
            actual.RouteValues.Should().ContainKey("id");
            actual.RouteValues["id"].Should().Be(id);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Not_Renewing_Key_Redirect_To_Hub_Provider(
            string employerAccountId,
            string id,
            int ukprn,
            RenewKeyViewModel viewModel,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            viewModel.ConfirmRenew = false;
            serviceParameters.Object.AuthenticationType = AuthenticationType.Provider;
            var controller = new SubscriptionsController(Mock.Of<IMediator>(), serviceParameters.Object);
            
            var actual = await controller.PostConfirmRenewKey(employerAccountId, id, ukprn, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.ProviderViewSubscription);
            actual.RouteValues.Should().ContainKey("ukprn");
            actual.RouteValues["ukprn"].Should().Be(ukprn);
            actual.RouteValues.Should().ContainKey("id");
            actual.RouteValues["id"].Should().Be(id);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Renewing_Key_Then_Send_Command_And_Redirect_With_Renewed_Param(
            string employerAccountId,
            string id,
            RenewKeyViewModel viewModel,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mockMediator)
        {
            viewModel.ConfirmRenew = true;
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            var controller = new SubscriptionsController(mockMediator.Object, serviceParameters.Object);
            
            var actual = await controller.PostConfirmRenewKey(employerAccountId, id, null, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.EmployerViewSubscription);
            actual.RouteValues.Should().ContainKey("employerAccountId");
            actual.RouteValues["employerAccountId"].Should().Be(employerAccountId);
            actual.RouteValues.Should().ContainKey("id");
            actual.RouteValues["id"].Should().Be(id);
            actual.RouteValues.Should().ContainKey("keyRenewed");
            actual.RouteValues["keyRenewed"].Should().Be(true);
            mockMediator.Verify(mediator => mediator.Send(
                It.Is<RenewSubscriptionKeyCommand>(command =>
                    command.AccountIdentifier.Equals(employerAccountId)
                    && command.ProductId.Equals(id)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Renewing_Key_Then_Send_Command_And_Redirect_With_Renewed_Param_For_Provider(
            string employerAccountId,
            string id,
            int ukprn,
            RenewKeyViewModel viewModel,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            viewModel.ConfirmRenew = true;
            serviceParameters.Object.AuthenticationType = AuthenticationType.Provider;
            var controller = new SubscriptionsController(mockMediator.Object, serviceParameters.Object);
            
            var actual = await controller.PostConfirmRenewKey(employerAccountId, id, ukprn, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.ProviderViewSubscription);
            actual.RouteValues.Should().ContainKey("ukprn");
            actual.RouteValues["ukprn"].Should().Be(ukprn);
            actual.RouteValues.Should().ContainKey("id");
            actual.RouteValues["id"].Should().Be(id);
            actual.RouteValues.Should().ContainKey("keyRenewed");
            actual.RouteValues["keyRenewed"].Should().Be(true);
            mockMediator.Verify(mediator => mediator.Send(
                    It.Is<RenewSubscriptionKeyCommand>(command =>
                        command.AccountIdentifier.Equals(ukprn.ToString())
                        && command.ProductId.Equals(id)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}