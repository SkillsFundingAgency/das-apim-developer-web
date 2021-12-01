using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Products.Queries.GetProduct;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Documentation
{
    public class WhenGettingApiDocumentation
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_Data_Returned(
            string apiName,
            GetProductQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DocumentationController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetProductQuery>(c => c.Id.Equals(apiName)), CancellationToken.None))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.GetApiProductDocumentation(apiName) as JsonResult;

            Assert.IsNotNull(actual);
            actual.Value.Should().BeEquivalentTo(mediatorResult.Product.Documentation);
            
        }
    }
}