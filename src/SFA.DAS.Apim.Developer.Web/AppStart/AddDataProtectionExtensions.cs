using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class AddDataProtectionExtensions
    {
        public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration, AuthenticationType? authenticationType)
        {
            
            var config = configuration.GetSection(nameof(ApimDeveloperWeb))
                .Get<ApimDeveloperWeb>();

            if (config != null 
                && !string.IsNullOrEmpty(config.DataProtectionKeysDatabase) 
                && !string.IsNullOrEmpty(config.RedisConnectionString))
            {
                var applicationName = authenticationType switch
                {
                    AuthenticationType.Employer => "das-employer",
                    AuthenticationType.Provider => "das-provider",
                    _ => "das-external"
                };

                var redisConnectionString = config.RedisConnectionString;
                var dataProtectionKeysDatabase = config.DataProtectionKeysDatabase;

                var redis = ConnectionMultiplexer
                    .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

                services.AddDataProtection()
                    .SetApplicationName(applicationName)
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
            }
        }
    }
}