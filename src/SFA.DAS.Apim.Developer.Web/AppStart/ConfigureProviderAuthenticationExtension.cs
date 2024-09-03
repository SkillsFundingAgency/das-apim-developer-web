using Microsoft.AspNetCore.Authentication;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class ConfigureProviderStubAuthentication
    {
        public static void AddProviderStubAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Provider-stub").AddScheme<AuthenticationSchemeOptions, ProviderStubAuthHandler>(
                "Provider-stub",
                options => { });
        }
    }
}