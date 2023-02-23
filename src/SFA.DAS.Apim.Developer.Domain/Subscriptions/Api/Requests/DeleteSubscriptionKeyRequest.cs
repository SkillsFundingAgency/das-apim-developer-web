using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests
{
    public class DeleteSubscriptionKeyRequest : IDeleteApiRequest
    {
        private readonly string _accountIdentifier;
        private readonly string _productId;

        public DeleteSubscriptionKeyRequest(string accountIdentifier, string productId)
        {
            _accountIdentifier = accountIdentifier;
            _productId = productId;
        }
        
        public string DeleteUrl => $"subscriptions/{_accountIdentifier}/delete/{_productId}";
    }
}