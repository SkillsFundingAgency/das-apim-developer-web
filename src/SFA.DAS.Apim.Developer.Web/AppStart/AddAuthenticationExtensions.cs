using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Application.Employer.Services;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.Api;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class AddAuthenticationExtensions
    {
        public static void AddEmployerAuthenticationServices(
            this IServiceCollection services)
        {
            services.AddTransient<IEmployerAccountAuthorisationHandler, EmployerAccountAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, EmployerAccountAuthorizationHandler>();
        }

        public static void AddProviderAuthenticationServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, ProviderAccountAuthorizationHandler>();
        }
    }
}