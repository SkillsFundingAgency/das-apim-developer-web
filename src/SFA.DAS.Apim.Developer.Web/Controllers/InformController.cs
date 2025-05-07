using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    
    public class InformController(IOptions<ApimDeveloperWeb> configuration) : Controller
    {
        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasEmployerViewAccount))]
        [Route("accounts/{employerAccountId}/recruitment/api", Name = RouteNames.EmployerRecruitInform)]
        [Employer.Shared.UI.Attributes.SetNavigationSection(Employer.Shared.UI.NavigationSection.RecruitHome)]
        public IActionResult Index([FromRoute]string employerAccountId)
        {
            return View("Index", new InformViewModel{EmployerAccountId = employerAccountId, DocumentationUrl = configuration.Value.DocumentationBaseUrl});
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
        [Route("{ukprn}/recruitment/api", Name = RouteNames.ProviderRecruitInform)]
        [SetNavigationSection(NavigationSection.Recruit)]
        public IActionResult ProviderIndex([FromRoute]int ukprn)
        {
            return View("ProviderIndex", new InformViewModel{Ukprn = ukprn, DocumentationUrl = configuration.Value.DocumentationBaseUrl});
        }
    }
}