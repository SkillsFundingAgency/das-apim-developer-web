using Newtonsoft.Json;

namespace SFA.DAS.Apim.Developer.Domain.Products.Api.Responses
{
    public class GetProductResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("documentation")]
        public string Documentation { get; set; }
    }
}