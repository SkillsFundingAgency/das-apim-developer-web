using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    [Route("third-party-accounts")]
    public class ThirdPartyAccountsController : Controller
    {
        [HttpGet]
        [Route("register", Name = RouteNames.ThirdPartyRegister)]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [Route("register", Name = RouteNames.ThirdPartyRegister)]
        public async Task<IActionResult> PostRegister(RegisterRequest request)
        {
            // call mediator and validate
            return View("Register");
        }
    }
}