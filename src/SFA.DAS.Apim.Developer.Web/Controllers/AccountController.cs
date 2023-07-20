using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [Route("signout",Name = RouteNames.ProviderSignOut)]
        public IActionResult ProviderSignOut()
        {
            var useDfESignIn = _configuration["ApimDeveloperWeb:UseDfESignIn"] != null &&
                               _configuration["ApimDeveloperWeb:UseDfESignIn"]
                                   .Equals("true", StringComparison.CurrentCultureIgnoreCase);

            var authScheme = useDfESignIn
                ? OpenIdConnectDefaults.AuthenticationScheme
                : WsFederationDefaults.AuthenticationScheme;

            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                },CookieAuthenticationDefaults.AuthenticationScheme, authScheme);
        }
    }
}