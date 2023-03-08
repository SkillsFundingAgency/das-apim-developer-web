using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.Api;

namespace SFA.DAS.Apim.Developer.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request);
        Task<ApiResponse<TResponse>> Post<TResponse>(IPostApiRequest request);
        Task<ApiResponse<TResponse>> Put<TResponse>(IPutApiRequest request);
        Task<ApiResponse<TResponse>> Delete<TResponse>(IDeleteApiRequest request);
    }
}