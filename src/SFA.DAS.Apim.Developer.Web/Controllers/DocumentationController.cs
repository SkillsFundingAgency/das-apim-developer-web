using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    
    [Route("[controller]")]
    public class DocumentationController : Controller
    {
        [HttpGet]
        [Route("recruitment", Name = RouteNames.RecruitDocumentation)]
        public IActionResult Recruitment()
        {
            return View();
        }
    }
}