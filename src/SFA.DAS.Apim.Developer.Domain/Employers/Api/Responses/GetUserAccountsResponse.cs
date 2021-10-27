using System.Collections.Generic;

namespace SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses
{
    public class GetUserAccountsResponse
    {
        public List<EmployerIdentifier> UserAccounts { get; set; }
    }
    
    public class EmployerIdentifier
    {
        public string AccountId { get; set; }
        public string EmployerName { get; set; }
        public string Role { get; set; }
    }
}