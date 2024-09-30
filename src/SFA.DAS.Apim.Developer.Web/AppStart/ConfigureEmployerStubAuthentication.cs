using Microsoft.AspNetCore.Authentication;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
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