using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.Subscriptions.Queries
{
    public class WhenHandlingGetAvailableProductsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Api_Called(
            GetAvailableProductsQuery query,
            GetAvailableProductSubscriptionsResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            GetAvailableProductsQueryHandler handler)
        {
            apiClient.Setup(c =>
                c.Get<GetAvailableProductSubscriptionsResponse>(
                    It.Is<GetAvailableProductSubscriptionsRequest>(c =>
                        c.GetUrl.EndsWith($"accountType={query.AccountType}")))).ReturnsAsync(new ApiResponse<GetAvailableProductSubscriptionsResponse>(apiResponse, HttpStatusCode.OK, ""));

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Products.Should().BeEquivalentTo(apiResponse);
        }
    }
}