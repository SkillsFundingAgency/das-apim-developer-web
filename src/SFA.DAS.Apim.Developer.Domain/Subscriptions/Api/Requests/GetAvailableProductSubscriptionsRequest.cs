using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests
{
    public class GetAvailableProductSubscriptionsRequest : IGetApiRequest
    {
        private readonly string _accountType;

        public GetAvailableProductSubscriptionsRequest(string accountType)
        {
            _accountType = accountType;
        }

        public string GetUrl => $"subscriptions/products?accountType={_accountType}";
    }
}