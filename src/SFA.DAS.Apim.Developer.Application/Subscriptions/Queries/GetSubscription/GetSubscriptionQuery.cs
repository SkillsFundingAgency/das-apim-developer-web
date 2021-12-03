using MediatR;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetSubscription
{
    public class GetSubscriptionQuery : IRequest<GetSubscriptionQueryResult>
    {
        public string AccountIdentifier { get ; set ; }
        public string ProductId { get ; set ; }
        public string AccountType { get ; set ; }
    }
}