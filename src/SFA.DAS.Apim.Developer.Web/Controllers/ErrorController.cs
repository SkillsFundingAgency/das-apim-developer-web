using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Infrastructure.Configuration;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        private readonly ExternalLinksConfiguration _configuration;

        public ErrorController (IOptions<ExternalLinksConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }
        [Route("403", Name = RouteNames.Error403)]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied", _configuration.ManageApprenticeshipSiteUrl);
        }
    }
}