using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Providers.Api.Requests
{
    public class GetProviderDetails : IGetApiRequest
    {
        private readonly long _ukprn;

        public GetProviderDetails(long ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"api/providers/{_ukprn}";
    }
}
