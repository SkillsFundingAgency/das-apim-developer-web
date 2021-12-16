using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class SubscriptionRouteModel
    {   public string ViewSubscriptionRouteName { get; }
        public string RenewKeyRouteName { get; }
        public string CreateKeyRouteName { get; }
        public string AccountIdentifier { get; }
        
        public SubscriptionRouteModel (ServiceParameters serviceParameters, string employerAccountId, int? ukprn, string externalId)
        {
            switch (serviceParameters.AuthenticationType)
            {
                case AuthenticationType.Employer:
                    ViewSubscriptionRouteName = RouteNames.EmployerViewSubscription;
                    RenewKeyRouteName = RouteNames.EmployerRenewKey;
                    CreateKeyRouteName = RouteNames.EmployerCreateKey;
                    AccountIdentifier = employerAccountId;
                    break;
                case AuthenticationType.Provider:
                    ViewSubscriptionRouteName = RouteNames.ProviderViewSubscription;
                    RenewKeyRouteName = RouteNames.ProviderRenewKey;
                    CreateKeyRouteName = RouteNames.ProviderCreateKey;
                    AccountIdentifier = ukprn.ToString();
                    break;
                case AuthenticationType.External:
                    ViewSubscriptionRouteName = RouteNames.ExternalViewSubscription;
                    RenewKeyRouteName = RouteNames.ExternalRenewKey;
                    CreateKeyRouteName = RouteNames.ExternalCreateKey;
                    AccountIdentifier = externalId;
                    break;
            }
        }
    }
}