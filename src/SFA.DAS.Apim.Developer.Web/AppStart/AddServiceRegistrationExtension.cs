using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Application.Employer.Services;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.Api;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services, ServiceParameters serviceParameters, IConfiguration configuration)
        {
            services.AddSingleton(serviceParameters);
            services.AddHttpContextAccessor();
            services.AddHttpClient<IApiClient, ApiClient>();
            services.AddTransient<IEmployerAccountService, EmployerAccountService>();
            services.AddTransient<IEmployerAccountAuthorisationHandler, EmployerAccountAuthorizationHandler>();
        }
    }
}