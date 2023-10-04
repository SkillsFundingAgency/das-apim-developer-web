using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Providers.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Providers.Api.Responses;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFA.DAS.Apim.Developer.Application.Provider.Services
{
    /// <inheritdoc />
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly TrainingProviderApiClientConfiguration _configuration;

        public TrainingProviderService(
            HttpClient client,
            TrainingProviderApiClientConfiguration configuration)
        {
            _httpClient = client;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<GetProviderSummaryResult> GetProviderDetails(long ukprn)
        {
            var getRequest = new GetProviderDetails(ukprn);
            var url = $"{BaseUrl()}{getRequest.GetUrl}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            await AddAuthenticationHeader(requestMessage);

            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<GetProviderSummaryResult>(json);
                case HttpStatusCode.NotFound:
                default:
                    return default;
            }
        }

        private string BaseUrl()
        {
            if (_configuration.ApiBaseUrl.EndsWith("/"))
            {
                return _configuration.ApiBaseUrl;
            }
            return _configuration.ApiBaseUrl + "/";
        }

        private async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            if (!string.IsNullOrEmpty(_configuration.IdentifierUri))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_configuration.IdentifierUri);
                httpRequestMessage.Headers.Remove("X-Version");
                httpRequestMessage.Headers.Add("X-Version", _configuration.Version);
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}
