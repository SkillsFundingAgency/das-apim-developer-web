using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Application.Employer.Services;
using SFA.DAS.Apim.Developer.Application.Provider.Services;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Services;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.Api;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.TagHelpers;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(
            this IServiceCollection services, 
            ServiceParameters serviceParameters, 
            IConfiguration configuration)
        {
            services.AddSingleton(serviceParameters);
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddHttpClient<IApiClient, ApiClient>();
            services.AddTransient<IApiDescriptionHelper, ApiDescriptionHelper>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITrainingProviderService, TrainingProviderService>();

            var useDevDataProtector = configuration["DevDataProtector"] != null 
                             && configuration["DevDataProtector"].Equals("true", StringComparison.CurrentCultureIgnoreCase);
            if (useDevDataProtector)
            {
                services.AddTransient<IDataProtectorService, DevDataProtectorService>();
            }
            else
            {
                services.AddTransient<IDataProtectorService, DataProtectorService>();
            } 
        }
    }
}