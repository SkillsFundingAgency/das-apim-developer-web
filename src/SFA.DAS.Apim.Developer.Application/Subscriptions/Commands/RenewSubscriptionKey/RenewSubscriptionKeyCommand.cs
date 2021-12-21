using MediatR;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.RenewSubscriptionKey
{
    public class RenewSubscriptionKeyCommand : IRequest<Unit>
    {
        public string AccountIdentifier { get; set; }
        public string ProductId { get; set; }
    }
}