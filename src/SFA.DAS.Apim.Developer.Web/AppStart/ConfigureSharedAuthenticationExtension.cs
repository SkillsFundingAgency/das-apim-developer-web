using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class ConfigureSharedAuthenticationExtension
    {
        public static void AddAuthenticationCookie(this IServiceCollection services,
            AuthenticationType? serviceParametersAuthenticationType)
        {
            services.AddAuthentication().AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/error/403");
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Cookie.Name = $"SFA.DAS.Apim.Developer.Web.{serviceParametersAuthenticationType}Auth";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.CookieManager = new ChunkingCookieManager { ChunkSize = 3000 };
                if (serviceParametersAuthenticationType == AuthenticationType.External)
                {
                    options.LoginPath = "/third-party-accounts/sign-in";
                    options.LogoutPath = "";
                }
            });
        }
    }
}