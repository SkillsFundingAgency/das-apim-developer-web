using System;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests
{
    public class PostRegisterThirdPartyAccountRequest  : IPostApiRequest
    {
        private Guid _id;
        public PostRegisterThirdPartyAccountRequest(PostRegisterThirdPartyAccountData data)
        {
            _id = data.Id;
            Data = data;
        }

        public string PostUrl => $"users/{_id}";
        public object Data { get; set; }
    }

    public class PostRegisterThirdPartyAccountData
    {
        public PostRegisterThirdPartyAccountData(IRegisterThirdPartyAccountData source)
        {
            Id = source.Id;
            FirstName = source.FirstName;
            LastName = source.LastName;
            EmailAddress = source.EmailAddress;
            Password = source.Password;
            ConfirmUrl = source.ConfirmUrl;
        }
        
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmUrl { get; set; }
    }
}