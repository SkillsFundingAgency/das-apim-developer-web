using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.Configuration;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class ConfigureEmployerAuthenticationExtension
    {
        public static void AddAndConfigureEmployerAuthentication(
            this IServiceCollection services,
            IdentityServerConfiguration configuration)
        {
            services
                .AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;

                }).AddOpenIdConnect(options =>
                {
                    options.ClientId = configuration.ClientId;
                    options.ClientSecret = configuration.ClientSecret;
                    options.Authority = configuration.BaseAddress;
                    options.MetadataAddress = $"{configuration.BaseAddress}/.well-known/openid-configuration";
                    options.ResponseType = "code";
                    options.UsePkce = false;
                    
                    var scopes = configuration.Scopes.Split(' ');
                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope);
                    }
                    options.ClaimActions.MapUniqueJsonKey("sub", "id");
                    options.Events.OnRemoteFailure = c =>
                    {
                        if (c.Failure.Message.Contains("Correlation failed"))
                        {
                            c.Response.Redirect("/");
                            c.HandleResponse();
                        }
    
                        return Task.CompletedTask;
                    };
                });
            services
                .AddOptions<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme)
                .Configure<IEmployerAccountService>((options, accountsService) =>
                {
                    options.Events.OnTokenValidated = async (ctx) => await PopulateAccountsClaim(ctx, accountsService);
                });
        }

        private static async Task PopulateAccountsClaim(TokenValidatedContext ctx, IEmployerAccountService accountsSvc)
        {
            var userId = ctx.Principal.Claims
                .First(c => c.Type.Equals(EmployerClaims.IdamsUserIdClaimTypeIdentifier))
                .Value;
            
            var result = await accountsSvc.GetUserAccounts(userId);
            var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
            var associatedAccountsClaim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);
            ctx.Principal.Identities.First().AddClaim(associatedAccountsClaim);
        }
    }
}