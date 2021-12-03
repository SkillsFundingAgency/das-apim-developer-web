using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.Apim.Developer.Domain.Extensions;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Subscriptions
{
    public class WhenCreatingSubscription
    {
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_Mediator_And_Redirected_To_View_Subscription_For_Employer(
            string id, 
            string employerAccountId,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.CreateSubscription(employerAccountId, id, null) as RedirectToRouteResult;
            
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.EmployerViewSubscription);
            actual.RouteValues["employerAccountId"].Should().Be(employerAccountId);
            actual.RouteValues["id"].Should().Be(id);
            mediator.Verify(x =>
                x.Send(It.Is<CreateSubscriptionKeyCommand>(c => 
                    c.AccountIdentifier.Equals(employerAccountId)
                    && c.AccountType.Equals(AuthenticationType.Employer.GetDescription())
                    && c.ProductId.Equals(id)
                ), CancellationToken.None), Times.Once);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_Mediator_And_Redirected_To_View_Subscription_For_Provider(
            string id, 
            int ukprn,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            serviceParameters.Object.AuthenticationType = AuthenticationType.Provider;
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.CreateSubscription("", id, ukprn) as RedirectToRouteResult;
            
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ProviderViewSubscription);
            actual.RouteValues["ukprn"].Should().Be(ukprn);
            actual.RouteValues["id"].Should().Be(id);
            mediator.Verify(x =>
                x.Send(It.Is<CreateSubscriptionKeyCommand>(c => 
                    c.AccountIdentifier.Equals(ukprn.ToString())
                    && c.AccountType.Equals(AuthenticationType.Provider.GetDescription())
                    && c.ProductId.Equals(id)
                ), CancellationToken.None), Times.Once);
        }
    }
}
