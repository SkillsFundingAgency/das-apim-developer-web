using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly ServiceParameters _serviceParameters;

        public AccountController (ServiceParameters serviceParameters)
        {
            _serviceParameters = serviceParameters;
        }
        
        [Route("signout",Name = RouteNames.ProviderSignOut)]
        public IActionResult SignOut()
        {
            var schemes = new List<string>
            {
                CookieAuthenticationDefaults.AuthenticationScheme
            };

            if(_serviceParameters.AuthenticationType == AuthenticationType.Provider)
            {
                schemes.Add(WsFederationDefaults.AuthenticationScheme);
            }

            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                },schemes.ToArray());
        }
    }
}