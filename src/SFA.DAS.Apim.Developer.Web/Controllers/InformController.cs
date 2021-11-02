using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    
    public class InformController : Controller
    {
        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
        [Route("accounts/{employerAccountId}/recruitment/api", Name = RouteNames.RecruitInform)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
        [Route("{ukprn}/recruitment/api", Name = RouteNames.ProviderRecruitInform)]
        public IActionResult ProviderIndex()
        {
            return View();
        }
    }
}