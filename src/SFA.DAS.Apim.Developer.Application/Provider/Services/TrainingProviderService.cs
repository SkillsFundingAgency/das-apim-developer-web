using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Providers.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Providers.Api.Responses;
using System.Threading.Tasks;

namespace SFA.DAS.Apim.Developer.Application.Provider.Services
{
    /// <inheritdoc />
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IApiClient _apiClient;

        public TrainingProviderService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<ProviderAccountResponse> GetProviderStatus(long ukprn)
        {
            var result = await _apiClient.Get<ProviderAccountResponse>(new GetProviderStatusDetails(ukprn));

            return result.Body;
        }
    }
}
