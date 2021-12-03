using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Products.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Products.Api.Responses;

namespace SFA.DAS.Apim.Developer.Application.Products.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductQueryResult>
    {
        private readonly IApiClient _apiClient;

        public GetProductQueryHandler (IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetProductQueryResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetProductResponse>(new GetProductRequest(request.Id));

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return new GetProductQueryResult
                {
                    Product = null
                };
            }
            
            return new GetProductQueryResult
            {
                Product = result.Body
            };
        }
    }
}