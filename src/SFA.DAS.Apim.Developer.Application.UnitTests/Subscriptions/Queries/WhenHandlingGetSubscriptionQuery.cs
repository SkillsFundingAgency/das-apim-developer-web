using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetSubscription;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.Subscriptions.Queries
{
    public class WhenHandlingGetSubscriptionQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            GetSubscriptionQuery query,
            GetProductSubscriptionItem apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            GetSubscriptionQueryHandler handler)
        {
            var expectedGetRequest = new GetProductSubscriptionRequest(query.AccountIdentifier, query.ProductId, query.AccountType);
            apiClient.Setup(x =>
                    x.Get<GetProductSubscriptionItem>(
                        It.Is<GetProductSubscriptionRequest>(c => c.GetUrl.Equals(expectedGetRequest.GetUrl))))
                .ReturnsAsync(new ApiResponse<GetProductSubscriptionItem>(apiResponse, HttpStatusCode.OK, ""));

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Product.Should().BeEquivalentTo(apiResponse);
        }
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Null_Returned_If_Not_Found(
            GetSubscriptionQuery query,
            GetProductSubscriptionItem apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            GetSubscriptionQueryHandler handler)
        {
            var expectedGetRequest = new GetProductSubscriptionRequest(query.AccountIdentifier, query.ProductId, query.AccountType);
            apiClient.Setup(x =>
                    x.Get<GetProductSubscriptionItem>(
                        It.Is<GetProductSubscriptionRequest>(c => c.GetUrl.Equals(expectedGetRequest.GetUrl))))
                .ReturnsAsync(new ApiResponse<GetProductSubscriptionItem>(null, HttpStatusCode.NotFound, ""));

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Product.Should().BeNull();
        }
    }
}