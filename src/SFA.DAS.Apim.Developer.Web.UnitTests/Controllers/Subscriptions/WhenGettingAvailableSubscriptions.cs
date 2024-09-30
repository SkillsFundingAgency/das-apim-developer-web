using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Domain.Extensions;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Subscriptions
{
    public class WhenGettingAvailableSubscriptions
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Query_Handled_And_Data_Returned_For_Employer(
            string employerAccountId,
            int ukprn,
            GetAvailableProductsQueryResult mediatorResult,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            mediator.Setup(x =>
                x.Send(
                    It.Is<GetAvailableProductsQuery>(c => 
                        c.AccountType.Equals(AuthenticationType.Employer.GetDescription())
                        && c.AccountIdentifier.Equals(employerAccountId)),
                    CancellationToken.None)).ReturnsAsync(mediatorResult);
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.ApiHub(employerAccountId, ukprn) as ViewResult;
            
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Model as SubscriptionsViewModel;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Products.Select(c=>c.DisplayName).Should().BeEquivalentTo(mediatorResult.Products.Products.Select(c=>c.DisplayName));
            actualModel.EmployerAccountId.Should().Be(employerAccountId);
            actualModel.ShowRenewedBanner.Should().BeFalse();
            actualModel.CreateKeyRouteName.Should().Be(RouteNames.EmployerCreateKey);
            actualModel.ViewKeyRouteName.Should().Be(RouteNames.EmployerViewSubscription);
            actualModel.AuthenticationType.Should().Be(serviceParameters.Object.AuthenticationType);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Mediator_Query_Handled_And_Data_Returned_For_Provider(
            string employerAccountId,
            int ukprn,
            GetAvailableProductsQueryResult mediatorResult,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            serviceParameters.Object.AuthenticationType = AuthenticationType.Provider;
            mediator.Setup(x =>
                x.Send(
                    It.Is<GetAvailableProductsQuery>(c => 
                        c.AccountType.Equals(AuthenticationType.Provider.GetDescription())
                        && c.AccountIdentifier.Equals(ukprn.ToString())),
                    CancellationToken.None)).ReturnsAsync(mediatorResult);
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.ApiHub(employerAccountId, ukprn) as ViewResult;
            
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Model as SubscriptionsViewModel;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Products.Select(c=>c.DisplayName).Should().BeEquivalentTo(mediatorResult.Products.Products.Select(c=>c.DisplayName));
            actualModel.Ukprn.Should().Be(ukprn);
            actualModel.ShowRenewedBanner.Should().BeFalse();
            actualModel.CreateKeyRouteName.Should().Be(RouteNames.ProviderCreateKey);
            actualModel.ViewKeyRouteName.Should().Be(RouteNames.ProviderViewSubscription);
            actualModel.AuthenticationType.Should().Be(serviceParameters.Object.AuthenticationType);
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_Mediator_Query_Handled_And_Data_Returned_For_External(
            string employerAccountId,
            int ukprn,
            string externalId,
            GetAvailableProductsQueryResult mediatorResult,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            serviceParameters.Object.AuthenticationType = AuthenticationType.External;
            mediator.Setup(x =>
                x.Send(
                    It.Is<GetAvailableProductsQuery>(c => 
                        c.AccountType.Equals(AuthenticationType.External.GetDescription())
                        && c.AccountIdentifier.Equals(externalId)),
                    CancellationToken.None)).ReturnsAsync(mediatorResult);
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.ApiHub(employerAccountId, ukprn, externalId) as ViewResult;
            
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Model as SubscriptionsViewModel;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Products.Select(c=>c.DisplayName).Should().BeEquivalentTo(mediatorResult.Products.Products.Select(c=>c.DisplayName));
            actualModel.Ukprn.Should().Be(ukprn);
            actualModel.ShowRenewedBanner.Should().BeFalse();
            actualModel.CreateKeyRouteName.Should().Be(RouteNames.ExternalCreateKey);
            actualModel.ViewKeyRouteName.Should().Be(RouteNames.ExternalViewSubscription);
            actualModel.AuthenticationType.Should().Be(serviceParameters.Object.AuthenticationType);
        }
        
    }
}