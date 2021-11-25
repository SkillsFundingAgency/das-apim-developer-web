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
        public async Task Then_The_Data_Is_Returned_From_Mediator_And_View_Subscription_Shown(
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
            
            var actual = await controller.ViewProductSubscription(employerAccountId, id, true) as ViewResult;
            
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as SubscriptionViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Product.Should().BeEquivalentTo(mediatorResult.Product, options=> options.Excluding(c=>c.Name));
            actualModel.EmployerAccountId.Should().Be(employerAccountId);
            actualModel.ShowRenewedBanner.Should().BeTrue();
            
        }
    }
}