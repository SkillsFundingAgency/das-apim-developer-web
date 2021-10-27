using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.Employers.Api
{
    public class GetEmployerUserAccounts
    {
        public IEnumerable<GetEmployerUserAccountItem> EmployerAccounts { get ; set ; }

        public static implicit operator GetEmployerUserAccounts(GetUserAccountsResponse source)
        {
            return new GetEmployerUserAccounts
            {
                EmployerAccounts = source.UserAccounts.Select(c=>(GetEmployerUserAccountItem)c).ToList()
            };
        }
    }

    public class GetEmployerUserAccountItem
    {
        public string AccountId { get; set; }
        public string EmployerName { get; set; }
        public string Role { get; set; }
        
        public static implicit operator GetEmployerUserAccountItem(EmployerIdentifier source)
        {
            return new GetEmployerUserAccountItem
            {
                AccountId = source.AccountId,
                EmployerName = source.EmployerName,
                Role = source.Role
            };
        }
    }
}