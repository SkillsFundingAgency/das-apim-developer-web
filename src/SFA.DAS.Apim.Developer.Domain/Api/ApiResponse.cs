using System.Net;

namespace SFA.DAS.Apim.Developer.Domain.Api
{
    public class ApiResponse<TResponse>
    {
        public TResponse Body { get;  }
        public HttpStatusCode StatusCode { get; }
        public string ErrorContent { get ; }

        public ApiResponse (TResponse body, HttpStatusCode statusCode, string errorContent)
        {
            Body = body;
            StatusCode = statusCode;
            ErrorContent = errorContent;
        }
    }
}