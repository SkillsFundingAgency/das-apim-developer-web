using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Authentication;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class AuthorizationServiceExtension
    {
        private const string ProviderDaa = "DAA";
        private const string ProviderDab = "DAB";
        private const string ProviderDac = "DAC";
        private const string ProviderDav = "DAV";
        
        public static void AddAuthorizationService(this IServiceCollection services,
            AuthenticationType? serviceParametersAuthenticationType)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    PolicyNames
                        .HasEmployerAccount
                    , policy =>
                    {
                        policy.RequireClaim(EmployerClaims.AccountsClaimsTypeIdentifier);
                        policy.Requirements.Add(new EmployerAccountRequirement());
                        policy.RequireAuthenticatedUser();
                        policy.Requirements.Add(new AccountActiveRequirement());
                    });
                options.AddPolicy(
                    PolicyNames.HasEmployerViewAccount, policy =>
                    {
                        policy.RequireClaim(EmployerClaims.AccountsClaimsTypeIdentifier);
                        policy.Requirements.Add(new EmployerViewerRoleRequirement());
                        policy.RequireAuthenticatedUser();
                        policy.Requirements.Add(new AccountActiveRequirement());
                    });
                options.AddPolicy(
                    PolicyNames
                        .HasProviderAccount
                    , policy =>
                    {
                     
                        policy.RequireClaim(ProviderClaims.ProviderUkprn);
                        policy.RequireClaim(ProviderClaims.Service, ProviderDaa, ProviderDab, ProviderDac, ProviderDav);
                        policy.Requirements.Add(new ProviderAccountRequirement());
                        policy.Requirements.Add(new TrainingProviderAllRolesRequirement());
                        policy.RequireAuthenticatedUser();
                    });
                options.AddPolicy(PolicyNames.HasExternalAccount, policy =>
                {
                    policy.RequireClaim(ExternalUserClaims.Id);
                    policy.Requirements.Add(new ExternalAccountRequirement());
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(
                    PolicyNames.HasProviderEmployerAdminOrExternalAccount,
                    policy =>
                    {
                        if (serviceParametersAuthenticationType is AuthenticationType.Employer)
                        {
                            policy.RequireClaim(EmployerClaims.AccountsClaimsTypeIdentifier);
                            policy.Requirements.Add(new AccountActiveRequirement());
                        }
                        else if (serviceParametersAuthenticationType is AuthenticationType.Provider)
                        {
                            policy.RequireClaim(ProviderClaims.ProviderUkprn);
                            policy.RequireClaim(ProviderClaims.Service, ProviderDaa);
                            policy.Requirements.Add(new TrainingProviderAllRolesRequirement());
                        }
                        else if (serviceParametersAuthenticationType is AuthenticationType.External)
                        {
                            policy.RequireClaim(ExternalUserClaims.Id);
                        }
                        policy.Requirements.Add(new ProviderEmployerExternalAccountRequirement());
                        policy.RequireAuthenticatedUser();
                    });
            });
        }
    }
}