using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.Providers.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    /// <summary>
    /// Contract to interact with Training Provider(RoATP/APAR) Outer Api.
    /// </summary>
    public interface ITrainingProviderService
    {
        /// <summary>
        /// Contract to get the details of the Provider by given ukprn or provider Id.
        /// </summary>
        /// <param name="providerId">ukprn.</param>
        /// <returns>GetProviderSummaryResult.</returns>
        Task<GetProviderSummaryResult> GetProviderDetails(long providerId);
    }
}
