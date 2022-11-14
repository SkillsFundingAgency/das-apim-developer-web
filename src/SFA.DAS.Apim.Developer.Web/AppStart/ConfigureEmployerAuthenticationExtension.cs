using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.Configuration;
using SFA.DAS.GovUK.Auth.Services;

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
                .Configure<IEmployerAccountService, ICustomClaims>((options, accountsService, customClaims) =>
                {
                    options.Events.OnTokenValidated = async (ctx) =>
                    {
                        var claims = await customClaims.GetClaims(ctx);
                        ctx.Principal.Identities.First().AddClaims(claims);
                    };
                });
        }
    }
    
    public static class ConfigureEmployerStubAuthentication
    {
        public static void AddEmployerStubAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Employer-stub").AddScheme<AuthenticationSchemeOptions, EmployerStubAuthHandler>(
                "Employer-stub",
                options => { });
        }
    }
}