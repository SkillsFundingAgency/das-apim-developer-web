using Newtonsoft.Json;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses
{
    public class GetUserResponse
    {
        public GetUserResponseItem User { get; set; }
    }
    
    public class GetUserResponseItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }
}