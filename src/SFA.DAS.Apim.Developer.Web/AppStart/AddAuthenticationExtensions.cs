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
            services.AddHttpContextAccessor();
            services.AddHttpClient<IApiClient, ApiClient>();
            services.AddTransient<IEmployerAccountService, EmployerAccountService>();
            services.AddSingleton<IAuthorizationHandler, EmployerAccountAuthorizationHandler>();
        }
    }
}