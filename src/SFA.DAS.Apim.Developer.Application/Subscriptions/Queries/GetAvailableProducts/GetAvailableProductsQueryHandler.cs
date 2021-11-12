using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Responses;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts
{
    public class GetAvailableProductsQueryHandler : IRequestHandler<GetAvailableProductsQuery, GetAvailableProductsQueryResult>
    {
        private readonly IApiClient _apiClient;

        public GetAvailableProductsQueryHandler (IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<GetAvailableProductsQueryResult> Handle(GetAvailableProductsQuery request, CancellationToken cancellationToken)
        {
            var result =
                await _apiClient.Get<GetAvailableProductSubscriptionsResponse>(
                    new GetAvailableProductSubscriptionsRequest(request.AccountType));

            return new GetAvailableProductsQueryResult
            {
                Products = result.Body.Products
            };
        }
    }
}