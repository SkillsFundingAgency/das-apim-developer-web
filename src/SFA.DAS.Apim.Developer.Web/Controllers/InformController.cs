using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Infrastructure.Configuration;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    
    public class InformController : Controller
    {
        private readonly ExternalLinksConfiguration _configuration;

        public InformController(IOptions<ExternalLinksConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

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
        [SetNavigationSection(NavigationSection.Recruit)]
        public IActionResult ProviderIndex()
        {
            return View();
        }
    }
}