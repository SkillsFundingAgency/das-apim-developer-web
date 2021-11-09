using System;
using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.Employers.Api;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Application.Employer.Services
{
    public class EmployerAccountService : IEmployerAccountService
    {
        private readonly IApiClient _apiClient;

        public EmployerAccountService (IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetEmployerUserAccounts> GetUserAccounts(string userId)
        {
            var result = await _apiClient.Get<GetUserAccountsResponse>(new GetUserAccountsRequest(userId));

            return result.Body;
        }
    }
}