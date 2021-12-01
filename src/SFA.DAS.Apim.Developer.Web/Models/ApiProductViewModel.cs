using SFA.DAS.Apim.Developer.Application.Products.Queries.GetProduct;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class ApiProductViewModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Documentation { get; set; }
        
        public static implicit operator ApiProductViewModel(GetProductQueryResult source)
        {
            if (source.Product == null)
            {
                return null;
            }
            return new ApiProductViewModel
            {
                Id = source.Product.Id,
                DisplayName = source.Product.DisplayName,
                Name = source.Product.Name,
                Description = source.Product.Description,
                Documentation = source.Product.Documentation
            };
        }
    }
}