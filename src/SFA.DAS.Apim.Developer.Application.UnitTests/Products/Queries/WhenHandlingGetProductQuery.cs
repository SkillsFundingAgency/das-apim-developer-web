using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Products.Queries.GetProduct;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Products.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Products.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.Products.Queries
{
    public class WhenHandlingGetProductQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            GetProductQuery query,
            GetProductResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            GetProductQueryHandler handler)
        {
            var expectedGetRequest = new GetProductRequest(query.Id);
            apiClient.Setup(x =>
                    x.Get<GetProductResponse>(
                        It.Is<GetProductRequest>(c => c.GetUrl.Equals(expectedGetRequest.GetUrl))))
                .ReturnsAsync(new ApiResponse<GetProductResponse>(apiResponse, HttpStatusCode.OK, ""));

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Product.Should().BeEquivalentTo(apiResponse);
        }
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Null_Returned_If_Not_Found(
            GetProductQuery query,
            [Frozen] Mock<IApiClient> apiClient,
            GetProductQueryHandler handler)
        {
            var expectedGetRequest = new GetProductRequest(query.Id);
            apiClient.Setup(x =>
                    x.Get<GetProductResponse>(
                        It.Is<GetProductRequest>(c => c.GetUrl.Equals(expectedGetRequest.GetUrl))))
                .ReturnsAsync(new ApiResponse<GetProductResponse>(null, HttpStatusCode.NotFound, ""));

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Product.Should().BeNull();
        }
    }
}