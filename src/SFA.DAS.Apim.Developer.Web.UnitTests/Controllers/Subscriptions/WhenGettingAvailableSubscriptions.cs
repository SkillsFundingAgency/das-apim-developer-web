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
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Subscriptions
{
    public class WhenGettingAvailableSubscriptions
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Query_Handled_And_Data_Returned(
            GetAvailableProductsQueryResult mediatorResult,
            [Frozen] Mock<ServiceParameters> serviceParameters, 
            [Frozen] Mock<IMediator> mediator)
        {
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            mediator.Setup(x =>
                x.Send(
                    It.Is<GetAvailableProductsQuery>(c => c.AccountType.Equals(AuthenticationType.Employer.GetDescription())),
                    CancellationToken.None)).ReturnsAsync(mediatorResult);
            var controller = new SubscriptionsController(mediator.Object, serviceParameters.Object);
            
            var actual = await controller.ApiHub() as ViewResult;
            
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as SubscriptionsViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Products.Select(c=>c.DisplayName).Should().BeEquivalentTo(mediatorResult.Products.Products.Select(c=>c.DisplayName));
        }
    }
}