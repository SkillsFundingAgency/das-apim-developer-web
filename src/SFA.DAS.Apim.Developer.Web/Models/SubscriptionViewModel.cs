namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class SubscriptionViewModel
    {
        public string EmployerAccountId { get ; set ; }
        public SubscriptionItem Product { get; set; }
        public bool ShowRenewedBanner { get ; set ; }
    }
}