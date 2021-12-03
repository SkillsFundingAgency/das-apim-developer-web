using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Responses;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetSubscription
{
    public class GetSubscriptionQueryHandler : IRequestHandler<GetSubscriptionQuery, GetSubscriptionQueryResult>
    {
        private readonly IApiClient _apiClient;

        public GetSubscriptionQueryHandler (IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetSubscriptionQueryResult> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Get<GetProductSubscriptionItem>(
                new GetProductSubscriptionRequest(request.AccountIdentifier, request.ProductId, request.AccountType));

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new GetSubscriptionQueryResult
                {
                    Product = null
                };
            }

            return new GetSubscriptionQueryResult
            {
                Product = response.Body
            };
        }
    }
}