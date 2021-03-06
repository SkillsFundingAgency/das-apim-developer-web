using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class ConfigureExternalUserAuthenticationExtension
    {
        public static void AddAndConfigureExternalUserAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                });
        }
    }
    
    public static class ConfigureExternalStubAuthentication
    {
        public static void AddExternalStubAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("External-stub").AddScheme<AuthenticationSchemeOptions, ExternalStubAuthHandler>(
                "External-stub",
                options => { });
        }
    }
}