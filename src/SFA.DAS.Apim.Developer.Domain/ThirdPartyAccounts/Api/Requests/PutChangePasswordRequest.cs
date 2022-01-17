using System;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests
{
    public class PutChangePasswordRequest : IPutApiRequest
    {
        private readonly Guid _id;

        public PutChangePasswordRequest(Guid id, string password)
        {
            _id = id;
            Data = new PutChangePasswordRequestData(password);
        }

        public string PutUrl => $"users/{_id}/change-password";
        public object Data { get; set; }
    }

    public class PutChangePasswordRequestData
    {
        public PutChangePasswordRequestData(string password)
        {
            Password = password;
        }

        public string Password { get; set; }
    }
}