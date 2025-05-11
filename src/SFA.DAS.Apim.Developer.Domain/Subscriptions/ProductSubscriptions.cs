using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.Subscriptions
{
    public class ProductSubscriptions
    {
        public IEnumerable<ProductSubscriptionItem> Products { get ; set ; }

        public static implicit operator ProductSubscriptions(List<GetProductSubscriptionItem> source)
        {
            if (source == null)
            {
                return new ProductSubscriptions
                {
                    Products = new List<ProductSubscriptionItem>()
                };
            }
            
            return new ProductSubscriptions
            {
                Products = source.Select(c=>(ProductSubscriptionItem)c)
            };
        }
    }

    public class ProductSubscriptionItem
    {
        public string Id { get ; set ; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public List<string> Versions { get; set; }


        public static implicit operator ProductSubscriptionItem(GetProductSubscriptionItem source)
        {
            return new ProductSubscriptionItem
            {
                Id = source.Id,
                Key = source.Key,
                Description = source.Description,
                Name = source.Name,
                DisplayName = source.DisplayName,
                Versions = source.Versions
            };
        }
    }
}