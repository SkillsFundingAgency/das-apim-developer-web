using MediatR;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts
{
    public class GetAvailableProductsQuery : IRequest<GetAvailableProductsQueryResult>
    {
        public string AccountType { get; set; }
        public string AccountIdentifier { get ; set ; }
    }
}