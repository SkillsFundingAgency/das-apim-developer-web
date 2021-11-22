using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Responses
{
    public class GetAvailableProductSubscriptionsResponse
    {
        [JsonProperty("products")]
        public List<GetProductSubscriptionItem> Products { get; set; }
    }

    public class GetProductSubscriptionItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}