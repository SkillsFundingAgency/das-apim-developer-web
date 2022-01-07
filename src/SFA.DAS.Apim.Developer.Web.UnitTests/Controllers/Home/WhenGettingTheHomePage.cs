using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Home
{
    public class WhenGettingTheHomePage
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Called_To_Get_Products_And_Documentation_Base_Url_Set(
            string documentationBaseUrl,
            GetAvailableProductsQueryResult response,
            [Frozen] Mock<IOptions<ApimDeveloperWeb>> config,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            config.Object.Value.DocumentationBaseUrl = documentationBaseUrl;
            mediator.Setup(x=>x.Send(
                It.Is<GetAvailableProductsQuery>(c=>
                    c.AccountIdentifier.Equals(Guid.Empty.ToString())
                    && c.AccountType.Equals("Documentation")), CancellationToken.None))
                .ReturnsAsync(response);
            
            var actual = await controller.Index() as ViewResult;
            
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as HomePageViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.DocumentationBaseUrl.Should().Be(documentationBaseUrl);
        }
    }
}