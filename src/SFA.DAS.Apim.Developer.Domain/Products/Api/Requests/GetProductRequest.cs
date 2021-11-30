using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Domain.Products.Api.Requests
{
    public class GetProductRequest : IGetApiRequest
    {
        private readonly string _productId;

        public GetProductRequest(string productId)
        {
            _productId = productId;
        }

        public string GetUrl => $"products/{_productId}";
    }
}