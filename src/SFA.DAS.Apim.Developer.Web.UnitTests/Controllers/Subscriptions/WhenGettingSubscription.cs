using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetSubscription;
using SFA.DAS.Apim.Developer.Domain.Extensions;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Subscriptions
{
    public class WhenGettingSubscription
    {
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_Mediator_And_View_Subscription_Shown_For_Employer(
            string id, 
            string employerAccountId,
            GetSubscriptionQueryResult mediatorResult,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            mediator.Setup(x =>
                x.Send(It.Is<GetSubscriptionQuery>(c => 
                    c.AccountIdentifier.Equals(employerAccountId)
                    && c.AccountType.Equals(AuthenticationType.Employer.GetDescription())
                    && c.ProductId.Equals(id)
                ), CancellationToken.None)).ReturnsAsync(mediatorResult);
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.ViewProductSubscription(employerAccountId, id,null, true) as ViewResult;
            
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Model as SubscriptionViewModel;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Product.Should().BeEquivalentTo(mediatorResult.Product, options=> options
                .Excluding(c=>c.Name)
                .Excluding(c=>c.Versions));
            actualModel.EmployerAccountId.Should().Be(employerAccountId);
            actualModel.ShowRenewedBanner.Should().BeTrue();
            actualModel.RenewKeyRouteName.Should().Be(RouteNames.EmployerRenewKey);
            
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_Mediator_And_View_Subscription_Shown_For_Provider(
            string id, 
            string employerAccountId,
            int ukprn,
            GetSubscriptionQueryResult mediatorResult,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            mediator.Setup(x =>
                x.Send(It.Is<GetSubscriptionQuery>(c => 
                    c.AccountIdentifier.Equals(ukprn.ToString())
                    && c.AccountType.Equals(AuthenticationType.Provider.GetDescription())
                    && c.ProductId.Equals(id)
                ), CancellationToken.None)).ReturnsAsync(mediatorResult);
            serviceParameters.Object.AuthenticationType = AuthenticationType.Provider;
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.ViewProductSubscription(employerAccountId, id,ukprn, true) as ViewResult;
            
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Model as SubscriptionViewModel;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Product.Should().BeEquivalentTo(mediatorResult.Product, options=> options
                .Excluding(c=>c.Name)
                .Excluding(c=>c.Versions));
            actualModel.Ukprn.Should().Be(ukprn);
            actualModel.ShowRenewedBanner.Should().BeTrue();
            actualModel.RenewKeyRouteName.Should().Be(RouteNames.ProviderRenewKey);

        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_Mediator_And_View_Subscription_Shown_For_External(
            string id, 
            string employerAccountId,
            string externalId,
            int ukprn,
            GetSubscriptionQueryResult mediatorResult,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            mediator.Setup(x =>
                x.Send(It.Is<GetSubscriptionQuery>(c => 
                    c.AccountIdentifier.Equals(externalId)
                    && c.AccountType.Equals(AuthenticationType.External.GetDescription())
                    && c.ProductId.Equals(id)
                ), CancellationToken.None)).ReturnsAsync(mediatorResult);
            serviceParameters.Object.AuthenticationType = AuthenticationType.External;
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.ViewProductSubscription(employerAccountId, id,ukprn, true, externalId) as ViewResult;
            
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Model as SubscriptionViewModel;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Product.Should().BeEquivalentTo(mediatorResult.Product, options=> options
                .Excluding(c=>c.Name)
                .Excluding(c=>c.Versions));
            actualModel.Ukprn.Should().Be(ukprn);
            actualModel.ShowRenewedBanner.Should().BeTrue();
            actualModel.RenewKeyRouteName.Should().Be(RouteNames.ExternalRenewKey);
        }
    }
}