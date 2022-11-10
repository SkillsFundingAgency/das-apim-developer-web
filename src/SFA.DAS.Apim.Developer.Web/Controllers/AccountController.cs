using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Infrastructure.Configuration;
using SFA.DAS.Provider.Shared.UI.Models;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IOptions<ApimDeveloperWeb> _apimDeveloperWebConfiguration;

        public AccountController(IOptions<ApimDeveloperWeb> apimDeveloperWebConfiguration)
        {
            _apimDeveloperWebConfiguration = apimDeveloperWebConfiguration;
        }

        [Route("signout", Name = RouteNames.ProviderSignOut)]
        public async Task<IActionResult> ProviderSignOutAsync()
        {
            if (_apimDeveloperWebConfiguration.Value.UseDfESignIn.Equals(true))
            {
                var authenticationProperties = new AuthenticationProperties();
                authenticationProperties.Parameters.Clear();
                return SignOut(
                    authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
            }

            return SignOut(
            new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                RedirectUri = "",
                AllowRefresh = true
            }, CookieAuthenticationDefaults.AuthenticationScheme, WsFederationDefaults.AuthenticationScheme);
            
        }
    }
}