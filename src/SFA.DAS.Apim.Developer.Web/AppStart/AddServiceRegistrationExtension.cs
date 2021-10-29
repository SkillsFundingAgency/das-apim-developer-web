using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services, ServiceParameters serviceParameters, IConfiguration configuration)
        {
            services.AddSingleton(serviceParameters);
        }
    }
}