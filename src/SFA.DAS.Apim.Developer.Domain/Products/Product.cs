using SFA.DAS.Apim.Developer.Domain.Products.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.Products
{
    public class Product
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Documentation { get; set; }

        public static implicit operator Product(GetProductResponse source)
        {
            return new Product
            {
                Id = source.Id,
                Description = source.Description,
                Documentation = source.Documentation,
                Name = source.Name,
                DisplayName = source.DisplayName
            };
        }
    }
}