using MediatR;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.DeleteSubscriptionKey
{
    public class DeleteSubscriptionKeyCommand : IRequest<Unit>
    {
        public string AccountIdentifier { get; set; }
        public string ProductId { get; set; }
    }
}