using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests
{
    public class PostRenewSubscriptionKeyRequest : IPostApiRequest
    {
        private readonly string _accountIdentifier;
        private readonly string _productId;

        public PostRenewSubscriptionKeyRequest(string accountIdentifier, string productId)
        {
            _accountIdentifier = accountIdentifier;
            _productId = productId;
        }

        public string PostUrl => $"subscriptions/{_accountIdentifier}/renew/{_productId}";
        public object Data { get; set; }
    }
}