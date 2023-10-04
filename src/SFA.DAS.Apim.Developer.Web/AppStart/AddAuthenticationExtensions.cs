using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Application.Employer.Services;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.Api;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Authentication;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class AddAuthenticationExtensions
    {
        public static void AddEmployerAuthenticationServices(
            this IServiceCollection services)
        {
            
            services.AddSingleton<IAuthorizationHandler, EmployerAccountAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, EmployerViewerAuthorizationHandler>();
        }

        public static void AddProviderAuthenticationServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, ProviderAccountAuthorizationHandler>();
            services.AddSingleton<ITrainingProviderAuthorizationHandler, TrainingProviderAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, TrainingProviderAllRolesAuthorizationHandler>();
        }

        public static void AddExternalAuthenticationServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, ExternalAccountAuthorizationHandler>();
        }

        public static void AddSharedAuthenticationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmployerAccountAuthorisationHandler, EmployerAccountAuthorizationHandler>();
            services.AddTransient<IProviderAccountAuthorisationHandler, ProviderAccountAuthorizationHandler>();
            services.AddTransient<IExternalAccountAuthorizationHandler, ExternalAccountAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ProviderEmployerExternalAccountAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AccountActiveAuthorizationHandler>();//TODO remove after gov one login go live
            services.AddSingleton<ITrainingProviderAuthorizationHandler, TrainingProviderAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, TrainingProviderAllRolesAuthorizationHandler>();
        }
    }
}