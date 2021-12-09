using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests
{
    public class PostAuthenticateUserRequest : IPostApiRequest
    {
        public PostAuthenticateUserRequest (string email, string password)
        {
            Data = new PostAuthenticateUserRequestData
            {
                Email = email,
                Password = password
            };
        }

        public string PostUrl => "users/authenticate";
        public object Data { get; set; }

    }

    public class PostAuthenticateUserRequestData
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}