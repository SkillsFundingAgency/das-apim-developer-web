using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Domain.Subscriptions;
using SFA.DAS.Apim.Developer.Web.AppStart;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class SubscriptionsViewModel
    {
        public List<SubscriptionItem> Products { get; set; }
        public string EmployerAccountId { get ; set ; }
        public bool ShowRenewedBanner { get; set; }
        public bool ShowDeletedBanner { get; set; }
        public string ApiName { get; set; }
        public string ViewKeyRouteName { get; set; }
        public string CreateKeyRouteName { get; set; }
        public int? Ukprn { get ; set ; }
        public string ExternalId { get ; set ; }
        public AuthenticationType? AuthenticationType { get ; set ; }

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
        public string Id { get ; set ; }
        public string Key { get ; set ; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowDocumentationUrl { get; set; }
        public Dictionary<string, string> Versions { get; set; }

        public static implicit operator SubscriptionItem(ProductSubscriptionItem source)
        {
            return new SubscriptionItem
            {
                Id = source.Id,
                Key = source.Key,
                DisplayName = source.DisplayName,
                Name = source.Name.ToLower(),
                Description = source.Description,
                ShowDocumentationUrl = source.Versions.Count > 0,
                Versions = source.Versions.ToDictionary(c=>c[^2..], c=>c)
            };
        }
    }
}