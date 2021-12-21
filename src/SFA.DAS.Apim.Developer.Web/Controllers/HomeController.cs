using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController (IMediator mediator)
        {
            _mediator = mediator;
        }
            
        [HttpGet]
        [Route("",Name = RouteNames.Index)]

        public async Task <IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAvailableProductsQuery
            {
                AccountIdentifier = Guid.Empty.ToString(),
                AccountType = "Documentation"
            });
            var model = new HomePageViewModel
            {
                ApiProducts = result.Products.Products.Select(c=>(SubscriptionItem)c).ToList()
            };
            return View(model);
        }
    }
}