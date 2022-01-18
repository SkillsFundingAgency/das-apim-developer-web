using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts
{
    public class UserDetails
    {
        public static implicit operator UserDetails(GetUserResponseItem source)
        {
            return new UserDetails
            {
                Id = source.Id,
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
    }
}