using System.Collections.Generic;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class HomePageViewModel
    {
        public List<SubscriptionItem> ApiProducts {get;set;}
        public List<SubscriptionItem> ExternalProducts { get; set; }
        public string DocumentationBaseUrl { get ; set ; }
    }
    
}