using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Responses
{
    public class GetAvailableProductSubscriptionsResponse
    {
        [JsonProperty("products")]
        public List<ProductSubscriptionItem> Products { get; set; }
    }

    public class ProductSubscriptionItem
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}