using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    
    [Route("[controller]")]
    public class DocumentationController : Controller
    {
        [HttpGet]
        [Route("{apiName}", Name = RouteNames.Documentation)]
        public IActionResult Recruitment([FromRoute]string apiName)
        {
            return View();
        }
    }
}