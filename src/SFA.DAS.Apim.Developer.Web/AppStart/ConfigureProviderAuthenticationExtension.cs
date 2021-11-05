using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Infrastructure.Configuration;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class ConfigureProviderAuthenticationExtension
    {
        public static void AddAndConfigureProviderAuthentication(this IServiceCollection services,
            ProviderIdams configuration)
        {
            services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignInScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme =
                        WsFederationDefaults.AuthenticationScheme;
                })
                .AddWsFederation(options =>
                {
                    options.MetadataAddress = configuration.MetadataAddress;
                    options.Wtrealm = configuration.Wtrealm;
                    options.CallbackPath = "/{ukprn}/inform";
                    options.Events.OnSecurityTokenValidated = async (ctx) =>
                    {
                        await PopulateProviderClaims(ctx.HttpContext, ctx.Principal);
                    };
                });   
        }
        private static Task PopulateProviderClaims(HttpContext httpContext, ClaimsPrincipal principal)
        {
            var providerId = principal.Claims.First(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;
            var displayName = principal.Claims.First(c => c.Type.Equals(ProviderClaims.DisplayName)).Value;
            httpContext.Items.Add(ClaimsIdentity.DefaultNameClaimType,providerId);
            httpContext.Items.Add(ProviderClaims.DisplayName,displayName);
            principal.Identities.First().AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, providerId));
            principal.Identities.First().AddClaim(new Claim(ProviderClaims.DisplayName, displayName));
            return Task.CompletedTask;
        }
    }
}