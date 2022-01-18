using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserResponse>
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<GetUserQueryHandler> _logger;

        public GetUserQueryHandler(IApiClient apiClient, ILogger<GetUserQueryHandler> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        
        public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _apiClient.Get<GetUserResponse>(new GetUserRequest(request.Email));
            
            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                _logger.LogInformation($"Failed to get user for email:[{request.Email}]");
                return new GetUserResponse();
            }

            return apiResponse.Body;
        }
    }
}