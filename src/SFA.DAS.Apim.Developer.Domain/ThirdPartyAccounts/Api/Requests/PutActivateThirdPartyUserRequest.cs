using System;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests
{
    public class PutActivateThirdPartyUserRequest : IPutApiRequest
    {
        private readonly Guid _id;

        public PutActivateThirdPartyUserRequest(Guid Id)
        {
            _id = Id;
        }

        public string PutUrl => $"users/{_id}/activate";
        public object Data { get; set; }
    }
}