using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
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
            var authScheme = _apimDeveloperWeb.UseDfESignIn
                ? OpenIdConnectDefaults.AuthenticationScheme
                : WsFederationDefaults.AuthenticationScheme;

            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    //RedirectUri = "",
                    AllowRefresh = true
                },CookieAuthenticationDefaults.AuthenticationScheme, authScheme);
        }
    }
}