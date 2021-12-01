using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Products.Queries.GetProduct;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Documentation
{
    public class WhenViewingDocumentation
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

            var actual = await controller.GetApiProduct(apiName) as ViewResult;

            Assert.IsNotNull(actual);
            var actualModel = actual.Model as ApiProductViewModel;
            Assert.IsNotNull(actualModel);
        }
    }
}