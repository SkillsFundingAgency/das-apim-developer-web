using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly ApimDeveloperWeb _apimDeveloperWeb;

        public AccountController(IOptions<ApimDeveloperWeb> configuration)
        {
            _apimDeveloperWeb = configuration.Value;
        }

        [Route("signout",Name = RouteNames.ProviderSignOut)]
        public IActionResult ProviderSignOut()
        {
            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    AllowRefresh = true
                },CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}