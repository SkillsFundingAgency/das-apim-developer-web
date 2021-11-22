using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.RenewSubscriptionKey
{
    public class RenewSubscriptionKeyCommandHandler : IRequestHandler<RenewSubscriptionKeyCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public RenewSubscriptionKeyCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<Unit> Handle(RenewSubscriptionKeyCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new PostRenewSubscriptionKeyRequest(request.AccountIdentifier, request.ProductId);
            var apiResponse = await _apiClient.Post<string>(apiRequest);
            //todo: should check if successful or not in future story
            return Unit.Value;
        }
    }
}