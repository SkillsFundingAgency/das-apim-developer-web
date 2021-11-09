using System.Web;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Employers.Api.Requests
{
    public class GetUserAccountsRequest : IGetApiRequest
    {
        private readonly string _userId;

        public GetUserAccountsRequest(string userId)
        {
            _userId = userId;
        }

        public string GetUrl => $"accountusers/{_userId}/accounts";
    }
}