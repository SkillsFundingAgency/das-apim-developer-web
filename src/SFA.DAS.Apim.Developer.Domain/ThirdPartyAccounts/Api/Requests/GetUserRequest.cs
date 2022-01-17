using System.Web;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests
{
    public class GetUserRequest : IGetApiRequest
    {
        private readonly string _email;

        public GetUserRequest(string email)
        {
            _email = email;
        }
        
        public string GetUrl => $"users?email={HttpUtility.UrlEncode(_email)}";
    }
}