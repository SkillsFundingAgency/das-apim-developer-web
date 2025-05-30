using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.GovUK.Auth.Employer;

namespace SFA.DAS.Apim.Developer.Application.Employer.Services
{
    public class EmployerAccountService : IGovAuthEmployerAccountService
    {
        private readonly IApiClient _apiClient;

        public EmployerAccountService (IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<EmployerUserAccounts> GetUserAccounts(string userId, string email)
        {
            var result = await _apiClient.Get<GetUserAccountsResponse>(new GetUserAccountsRequest(userId, email));

            return new EmployerUserAccounts
            {
                EmployerAccounts = result.Body.UserAccounts != null? result.Body.UserAccounts.Select(c => new EmployerUserAccountItem
                {
                    Role = c.Role,
                    AccountId = c.AccountId,
                    ApprenticeshipEmployerType = Enum.Parse<ApprenticeshipEmployerType>(c.ApprenticeshipEmployerType.ToString()),
                    EmployerName = c.EmployerName,
                }).ToList() : [],
                FirstName = result.Body.FirstName,
                IsSuspended = result.Body.IsSuspended,
                LastName = result.Body.LastName,
                EmployerUserId = result.Body.EmployerUserId,
            };
        }
    }
}