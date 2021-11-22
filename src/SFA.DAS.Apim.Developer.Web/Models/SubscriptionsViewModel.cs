using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Domain.Subscriptions;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class SubscriptionsViewModel
    {
        public List<SubscriptionItem> Products { get; set; }
        public string EmployerAccountId { get ; set ; }
        public bool ShowRenewedBanner { get; set; }

        public static implicit operator SubscriptionsViewModel(GetAvailableProductsQueryResult source)
        {
            return new SubscriptionsViewModel
            {
                Products = source.Products.Products.Select(c=>(SubscriptionItem)c).ToList()

            };
        }
    }

    public class SubscriptionItem
    {
        public string Key { get ; set ; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public static implicit operator SubscriptionItem(ProductSubscriptionItem source)
        {
            return new SubscriptionItem
            {
                Key = source.Key,
                DisplayName = source.DisplayName,
                Name = source.Name.ToLower(),
                Description = source.Description
            };
        }
    }
}