using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.Employers.Api;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IEmployerAccountService
    {
        Task<GetEmployerUserAccounts> GetUserAccounts(string userId);
    }
}