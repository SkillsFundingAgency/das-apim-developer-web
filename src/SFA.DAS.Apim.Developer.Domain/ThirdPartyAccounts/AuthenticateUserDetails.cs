using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts
{
    public class AuthenticateUserDetails
    {
        public static implicit operator AuthenticateUserDetails(PostAuthenticateUserResponseItem source)
        {
            return new AuthenticateUserDetails
            {
                Id = source.Id,
                Authenticated = source.Authenticated,
                Email = source.Email,
                State = source.State,
                FirstName = source.FirstName,
                LastName = source.LastName
            };
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public bool Authenticated { get; set; }
    }
}