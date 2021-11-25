using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

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
                    });
                options.AddPolicy(
                    PolicyNames.HasEmployerViewAccount, policy =>
                    {
                        policy.RequireClaim(EmployerClaims.AccountsClaimsTypeIdentifier);
                        policy.Requirements.Add(new EmployerViewerRoleRequirement());
                        policy.RequireAuthenticatedUser();
                    });
                options.AddPolicy(
                    PolicyNames
                        .HasProviderAccount
                    , policy =>
                    {
                     
                        policy.RequireClaim(ProviderClaims.ProviderUkprn);
                        policy.RequireClaim(ProviderClaims.Service, ProviderDaa, ProviderDab, ProviderDac, ProviderDav);
                        policy.Requirements.Add(new ProviderAccountRequirement());
                        policy.RequireAuthenticatedUser();
                    });
                options.AddPolicy(
                    PolicyNames.HasProviderOrEmployerAccount,
                    policy =>
                    {
                        if (serviceParametersAuthenticationType is AuthenticationType.Employer)
                        {
                            policy.RequireClaim(EmployerClaims.AccountsClaimsTypeIdentifier);
                        }
                        else if (serviceParametersAuthenticationType is AuthenticationType.Provider)
                        {
                            policy.RequireClaim(ProviderClaims.ProviderUkprn);
                            policy.RequireClaim(ProviderClaims.Service, ProviderDaa, ProviderDab, ProviderDac, ProviderDav);
                        }
                        policy.Requirements.Add(new ProviderOrEmployerAccountRequirement());
                        policy.RequireAuthenticatedUser();
                    });
            });
        }
    }
}