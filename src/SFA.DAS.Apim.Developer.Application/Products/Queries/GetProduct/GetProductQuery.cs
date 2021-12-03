using MediatR;

namespace SFA.DAS.Apim.Developer.Application.Products.Queries.GetProduct
{
    public class GetProductQuery : IRequest<GetProductQueryResult>
    {
        public string Id { get; set; }
    }
}