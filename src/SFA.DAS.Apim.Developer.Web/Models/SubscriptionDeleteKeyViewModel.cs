using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class SubscriptionDeleteKeyViewModel
    {
        public string Id { get; set; }
        public string EmployerAccountId { get; set; }
        public string DeleteKeyRouteName { get; set; }
        public string PostSubmitDeleteKeyRouteName { get; set; }
        public int? Ukprn { get; set; }
        public string ExternalId { get; set; }
        public AuthenticationType? AuthenticationType { get; set; }

        public SubscriptionDeleteKeyViewModel(ServiceParameters serviceParameters, string id, string employerAccountId, int? ukprn, string externalId)
        {
            switch (serviceParameters.AuthenticationType)
            {
                case AppStart.AuthenticationType.Employer:
                    DeleteKeyRouteName = RouteNames.EmployerDeleteKey;
                    PostSubmitDeleteKeyRouteName = RouteNames.EmployerApiHub;
                    Id = id;
                    EmployerAccountId = employerAccountId;
                    Ukprn = ukprn;
                    ExternalId = externalId;
                    AuthenticationType = serviceParameters.AuthenticationType;
                    break;
                case AppStart.AuthenticationType.Provider:
                    DeleteKeyRouteName = RouteNames.ProviderDeleteKey;
                    PostSubmitDeleteKeyRouteName = RouteNames.ApiList;
                    Id = id;
                    EmployerAccountId = employerAccountId;
                    Ukprn = ukprn;
                    ExternalId = externalId;
                    AuthenticationType = serviceParameters.AuthenticationType;
                    break;
                case AppStart.AuthenticationType.External:
                    DeleteKeyRouteName = RouteNames.ExternalDeleteKey;
                    PostSubmitDeleteKeyRouteName = RouteNames.ApiList;
                    Id = id;
                    EmployerAccountId = employerAccountId;
                    Ukprn = ukprn;
                    ExternalId = externalId;
                    AuthenticationType = serviceParameters.AuthenticationType;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceParameters.AuthenticationType), serviceParameters.AuthenticationType, null);
            }
        }
    }
}