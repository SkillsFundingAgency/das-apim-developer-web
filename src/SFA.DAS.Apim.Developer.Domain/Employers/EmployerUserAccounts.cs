using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.Employers
{
    public class EmployerUserAccounts
    {
        public IEnumerable<EmployerUserAccountItem> EmployerAccounts { get ; set ; }

        public static implicit operator EmployerUserAccounts(GetUserAccountsResponse source)
        {
            if (source?.UserAccounts == null)
            {
                return new EmployerUserAccounts
                {
                    EmployerAccounts = new List<EmployerUserAccountItem>()
                };
            }
            
            return new EmployerUserAccounts
            {
                EmployerAccounts = source.UserAccounts.Select(c=>(EmployerUserAccountItem)c).ToList()
            };
        }
    }

    public class EmployerUserAccountItem
    {
        public string AccountId { get; set; }
        public string EmployerName { get; set; }
        public string Role { get; set; }
        
        public static implicit operator EmployerUserAccountItem(EmployerIdentifier source)
        {
            return new EmployerUserAccountItem
            {
                AccountId = source.AccountId,
                EmployerName = source.EmployerName,
                Role = source.Role
            };
        }
    }
}