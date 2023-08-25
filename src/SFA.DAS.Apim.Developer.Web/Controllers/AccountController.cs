using Microsoft.AspNetCore.Authentication;
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
        public async Task<IActionResult> ProviderSignOut()
        {
            if (_apimDeveloperWeb.UseDfESignIn)
            {
                var idToken = await HttpContext.GetTokenAsync("id_token");
                var authenticationProperties = new AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                };
                authenticationProperties.Parameters.Clear();
                authenticationProperties.Parameters.Add("id_token", idToken);
                return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
            }
            else
            {
                return SignOut(
                    new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                    {
                        RedirectUri = "",
                        AllowRefresh = true
                    }, CookieAuthenticationDefaults.AuthenticationScheme, WsFederationDefaults.AuthenticationScheme);
            }
        }
    }
}