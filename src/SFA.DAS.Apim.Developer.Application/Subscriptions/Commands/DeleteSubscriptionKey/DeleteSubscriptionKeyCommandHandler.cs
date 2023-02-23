using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.DeleteSubscriptionKey

{
    public class DeleteSubscriptionKeyCommandHandler : IRequestHandler<DeleteSubscriptionKeyCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public DeleteSubscriptionKeyCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<Unit> Handle(DeleteSubscriptionKeyCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new DeleteSubscriptionKeyRequest(request.AccountIdentifier, request.ProductId);
            var apiResponse = await _apiClient.Delete<string>(apiRequest);
            //todo: should check if successful or not in future story
            return Unit.Value;
        }
    }
}