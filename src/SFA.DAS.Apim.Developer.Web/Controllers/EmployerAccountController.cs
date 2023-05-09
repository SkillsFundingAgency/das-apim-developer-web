using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class EmployerAccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IStubAuthenticationService _stubAuthenticationService;

        public EmployerAccountController(IConfiguration config, IStubAuthenticationService stubAuthenticationService)
        {
            _config = config;
            _stubAuthenticationService = stubAuthenticationService;
        }
        
        [Route("sign-out", Name = RouteNames.EmployerSignOut)]
        public async Task<IActionResult> SignOutEmployer()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.Parameters.Clear();
            authenticationProperties.Parameters.Add("id_token",idToken);
            return SignOut(
                authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }


        [Route("signoutcleanup")]
        public void SignOutCleanup()
        {
            Response.Cookies.Delete("SFA.DAS.Apim.Developer.Web.EmployerAuth");
        }
        
#if DEBUG
        [HttpGet]
        [Route("SignIn-Stub")]
        public IActionResult SigninStub()
        {
            return View("SigninStub", new List<string>{_config["StubId"],_config["StubEmail"]});
        }
        [HttpPost]
        [Route("SignIn-Stub")]
        public IActionResult SigninStubPost()
        {
            var model =  new StubAuthUserDetails
            {
                Email = _config["StubEmail"],
                Id = _config["StubId"]
            };
            _stubAuthenticationService.AddStubEmployerAuth(Response.Cookies, model, true);

            return RedirectToRoute("Signed-in-stub");
        }

        [Authorize]
        [HttpGet]
        [Route("signed-in-stub", Name = "Signed-in-stub")]
        public IActionResult SignedInStub()
        {
            return View();
        }
#endif
    }
}