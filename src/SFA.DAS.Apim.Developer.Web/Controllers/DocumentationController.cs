using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Application.Products.Queries.GetProduct;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    [Route("[controller]")]
    public class DocumentationController : Controller
    {
        private readonly IMediator _mediator;

        public DocumentationController (IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("{apiName}", Name = RouteNames.Documentation)]
        public async Task<IActionResult> GetApiProduct([FromRoute]string apiName)
        {
            var result = await _mediator.Send(new GetProductQuery
            {
                Id = apiName
            });

            var model = (ApiProductViewModel)result;
            
            return View(model);
        }

        [HttpGet]
        [Route("{apiName}/description")]
        public async Task<IActionResult> GetApiProductDocumentation([FromRoute] string apiName)
        {
            var result = await _mediator.Send(new GetProductQuery
            {
                Id = apiName
            });

            return new JsonResult(result.Product.Documentation);
        }
    }
}