using MediatR;
using System;

namespace SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts
{
    public class GetAvailableProductsQuery : IRequest<GetAvailableProductsQueryResult>
    {
        public string AccountType { get; set; }
        public string AccountIdentifier { get ; set ; }

        public GetAvailableProductsQuery(string accountType)
            => (AccountType, AccountIdentifier) = (accountType, Guid.Empty.ToString());

        public GetAvailableProductsQuery(string accountType, string accountIdentifier)
            => (AccountType, AccountIdentifier) = (accountType, accountIdentifier);
    }
}