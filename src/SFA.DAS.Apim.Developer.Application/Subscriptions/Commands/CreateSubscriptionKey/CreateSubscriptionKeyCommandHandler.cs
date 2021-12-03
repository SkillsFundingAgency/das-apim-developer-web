using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.CreateSubscriptionKey
{
    public class CreateSubscriptionKeyCommandHandler : IRequestHandler<CreateSubscriptionKeyCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public CreateSubscriptionKeyCommandHandler (IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<Unit> Handle(CreateSubscriptionKeyCommand request, CancellationToken cancellationToken)
        {
            await _apiClient.Post<object>(new PostCreateSubscriptionRequest(request.AccountIdentifier,
                request.ProductId, request.AccountType));
            
            return Unit.Value;
        }
    }
}