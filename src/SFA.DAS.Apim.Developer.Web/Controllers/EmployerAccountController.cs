using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class EmployerAccountController : Controller
    {
        [Route("{employerAccountId}/signout", Name = RouteNames.EmployerSignOut)]
        public IActionResult SignOutEmployer()
        {
            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }


        [Route("signoutcleanup")]
        public void SignOutCleanup()
        {
            Response.Cookies.Delete("SFA.DAS.Apim.Developer.Web.EmployerAuth");
        }
    }
}