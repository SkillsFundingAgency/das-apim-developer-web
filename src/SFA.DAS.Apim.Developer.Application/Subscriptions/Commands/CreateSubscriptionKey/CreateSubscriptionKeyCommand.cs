using MediatR;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.CreateSubscriptionKey
{
    public class CreateSubscriptionKeyCommand : IRequest<Unit>
    {
        public string AccountIdentifier { get ; set ; }
        public string ProductId { get ; set ; }
        public string AccountType { get ; set ; }
    }
}