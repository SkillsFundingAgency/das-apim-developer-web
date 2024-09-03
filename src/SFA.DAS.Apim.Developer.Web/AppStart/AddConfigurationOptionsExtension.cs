using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Domain.Configuration;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(
            this IServiceCollection services,
            IConfiguration configuration,
            AuthenticationType? authenticationType)
        {
            services.Configure<ApimDeveloperWeb>(configuration.GetSection(nameof(ApimDeveloperWeb)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApimDeveloperWeb>>().Value);
            services.Configure<ApimDeveloperApi>(configuration.GetSection($"{authenticationType}{nameof(ApimDeveloperApi)}"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApimDeveloperApi>>().Value);
        }
    }
}