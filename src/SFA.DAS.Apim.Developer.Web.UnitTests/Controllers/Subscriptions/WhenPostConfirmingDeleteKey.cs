using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.DeleteSubscriptionKey;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Subscriptions
{
    public class WhenPostConfirmingDeleteKey
    {
        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Not_Valid_Then_Redirect_To_Confirm(
            string employerAccountId,
            string id,
            string externalId,
            DeleteKeyViewModel viewModel,
            [Greedy] SubscriptionsController controller)
        {
            controller.ModelState.AddModelError("key", "error message");

            var actual = await controller.PostConfirmDeleteKey(employerAccountId, id, null, externalId, viewModel) as ViewResult;

            actual.ViewName.Should().Be("ConfirmDeleteKey");
            actual.Model.Should().BeAssignableTo<AuthenticationType>();
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Not_Deleting_Key_Redirect_To_Hub_Employer(
            string employerAccountId,
            string id,
            string externalId,
            DeleteKeyViewModel viewModel,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            viewModel.ConfirmDelete = false;
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            var controller = new SubscriptionsController(Mock.Of<IMediator>(), serviceParameters.Object);

            var actual = await controller.PostConfirmDeleteKey(employerAccountId, id, null, externalId, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.ApiList);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Not_Deleting_Key_Redirect_To_Hub_Provider(
            string employerAccountId,
            string id,
            int ukprn,
            string externalId,
            DeleteKeyViewModel viewModel,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            viewModel.ConfirmDelete = false;
            serviceParameters.Object.AuthenticationType = AuthenticationType.Provider;
            var controller = new SubscriptionsController(Mock.Of<IMediator>(), serviceParameters.Object);

            var actual = await controller.PostConfirmDeleteKey(employerAccountId, id, ukprn, externalId, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.ApiList);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Deleting_Key_Then_Send_Command_And_Redirect_With_Deleted_Param(
            string employerAccountId,
            string id,
            string externalId,
            DeleteKeyViewModel viewModel,
            [Frozen] Mock<ServiceParameters> serviceParameters,
            [Frozen] Mock<IMediator> mockMediator)
        {
            viewModel.ConfirmDelete = true;
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            var controller = new SubscriptionsController(mockMediator.Object, serviceParameters.Object);

            var actual = await controller.PostConfirmDeleteKey(employerAccountId, id, null, externalId, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.ApiList);
            actual.RouteValues.Should().ContainKey("keyDeleted");
            actual.RouteValues["keyDeleted"].Should().Be(true);
            mockMediator.Verify(mediator => mediator.Send(
                It.Is<DeleteSubscriptionKeyCommand>(command =>
                    command.AccountIdentifier.Equals(employerAccountId)
                    && command.ProductId.Equals(id)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Deleting_Key_Then_Send_Command_And_Redirect_With_Deleted_Param_For_Provider(
            string employerAccountId,
            string id,
            int ukprn,
            string externalId,
            DeleteKeyViewModel viewModel,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            viewModel.ConfirmDelete = true;
            serviceParameters.Object.AuthenticationType = AuthenticationType.Provider;
            var controller = new SubscriptionsController(mockMediator.Object, serviceParameters.Object);

            var actual = await controller.PostConfirmDeleteKey(employerAccountId, id, ukprn, externalId, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.ApiList);
            actual.RouteValues.Should().ContainKey("keyDeleted");
            actual.RouteValues["keyDeleted"].Should().Be(true);
            mockMediator.Verify(mediator => mediator.Send(
                    It.Is<DeleteSubscriptionKeyCommand>(command =>
                        command.AccountIdentifier.Equals(ukprn.ToString())
                        && command.ProductId.Equals(id)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Model_Is_Valid_And_Deleting_Key_Then_Send_Command_And_Redirect_With_Deleted_Param_For_External(
            string employerAccountId,
            string id,
            int ukprn,
            string externalId,
            DeleteKeyViewModel viewModel,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            viewModel.ConfirmDelete = true;
            serviceParameters.Object.AuthenticationType = AuthenticationType.External;
            var controller = new SubscriptionsController(mockMediator.Object, serviceParameters.Object);

            var actual = await controller.PostConfirmDeleteKey(employerAccountId, id, ukprn, externalId, viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.ApiList);
            actual.RouteValues.Should().ContainKey("keyDeleted");
            actual.RouteValues["keyDeleted"].Should().Be(true);
            mockMediator.Verify(mediator => mediator.Send(
                    It.Is<DeleteSubscriptionKeyCommand>(command =>
                        command.AccountIdentifier.Equals(externalId)
                        && command.ProductId.Equals(id)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}