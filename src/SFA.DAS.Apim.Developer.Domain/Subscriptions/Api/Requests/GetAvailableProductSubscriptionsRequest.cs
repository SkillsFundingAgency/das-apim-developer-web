using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests
{
    public class GetAvailableProductSubscriptionsRequest : IGetApiRequest
    {
        private readonly string _accountType;
        private readonly string _accountIdentifier;

        public GetAvailableProductSubscriptionsRequest(string accountType, string accountIdentifier)
        {
            _accountType = accountType;
            _accountIdentifier = accountIdentifier;
        }

        public string GetUrl => $"subscriptions/{_accountIdentifier}/products?accountType={_accountType}";
    }
}