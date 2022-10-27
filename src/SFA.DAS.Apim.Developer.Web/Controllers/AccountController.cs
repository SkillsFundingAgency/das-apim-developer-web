using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        [Route("signout",Name = RouteNames.ProviderSignOut)]
        public IActionResult ProviderSignOut()
        {
            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                },CookieAuthenticationDefaults.AuthenticationScheme, WsFederationDefaults.AuthenticationScheme);
        }
    }
}