using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticateUserDetails> AuthenticateUser(string email, string password);
    }
}