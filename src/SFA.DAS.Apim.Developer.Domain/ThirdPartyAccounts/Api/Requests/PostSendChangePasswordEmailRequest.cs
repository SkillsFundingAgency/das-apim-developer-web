using System;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests
{
    public class PostSendChangePasswordEmailRequest : IPostApiRequest
    {
        private Guid _id;
        
        public PostSendChangePasswordEmailRequest(PostSendChangePasswordEmailRequestData data)
        {
            _id = data.Id;
            Data = data;
        }

        public string PostUrl => $"users/{_id}/send-change-password-email";
        public object Data { get; set; }
    }

    public class PostSendChangePasswordEmailRequestData
    {
        public PostSendChangePasswordEmailRequestData(ISendChangePasswordEmailData source)
        {
            Id = source.Id;
            FirstName = source.FirstName;
            LastName = source.LastName;
            Email = source.Email;
            ChangePasswordUrl = source.ChangePasswordUrl;
        }
        
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ChangePasswordUrl { get; set; }
    }
}