using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests
{
    public class GetProductSubscriptionRequest : IGetApiRequest
    {
        private readonly string _accountIdentifier;
        private readonly string _productId;
        private readonly string _accountType;

        public GetProductSubscriptionRequest(string accountIdentifier, string productId, string accountType)
        {
            _accountIdentifier = accountIdentifier;
            _productId = productId;
            _accountType = accountType;
        }

        public string GetUrl => $"subscriptions/{_accountIdentifier}/products/{_productId}?accountType={_accountType}";
    }
}