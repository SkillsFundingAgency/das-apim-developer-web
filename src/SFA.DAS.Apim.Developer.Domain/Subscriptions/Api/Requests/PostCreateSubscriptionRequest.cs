using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests
{
    public class PostCreateSubscriptionRequest : IPostApiRequest
    {
        private readonly string _accountIdentifier;
        private readonly string _productId;
        private readonly string _accountType;

        public PostCreateSubscriptionRequest(string accountIdentifier, string productId, string accountType)
        {
            _accountIdentifier = accountIdentifier;
            _productId = productId;
            _accountType = accountType;
            Data = new object();
        }

        public string PostUrl => $"subscriptions/{_accountIdentifier}/products/{_productId}?accountType={_accountType}";
        public object Data { get; set; }
    }
}