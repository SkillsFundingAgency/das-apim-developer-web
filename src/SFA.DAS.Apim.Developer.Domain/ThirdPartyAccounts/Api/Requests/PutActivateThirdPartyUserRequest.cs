using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests
{
    public class PutActivateThirdPartyUserRequest : IPutApiRequest
    {
        public string PutUrl { get; }
        public object Data { get; set; }
    }
}