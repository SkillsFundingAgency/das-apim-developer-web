using SFA.DAS.Apim.Developer.Web.AppStart;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class SubscriptionViewModel
    {
        public string EmployerAccountId { get ; set ; }
        public SubscriptionItem Product { get; set; }
        public bool ShowRenewedBanner { get ; set ; }
        public int? Ukprn { get ; set ; }
        public string RenewKeyRouteName { get ; set ; }
        public AuthenticationType? AuthenticationType { get ; set ; }
        public string ExternalId { get ; set ; }
    }
}