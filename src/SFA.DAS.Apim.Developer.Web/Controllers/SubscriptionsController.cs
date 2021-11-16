using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Domain.Extensions;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ServiceParameters _serviceParameters;

        public SubscriptionsController (IMediator mediator, ServiceParameters serviceParameters)
        {
            _mediator = mediator;
            _serviceParameters = serviceParameters;
        }
        
        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
        [Route("accounts/{employerAccountId}/[controller]", Name = RouteNames.ApiHub)]
        public async Task<IActionResult> ApiHub([FromRoute]string employerAccountId)
        {
            var result = await _mediator.Send(new GetAvailableProductsQuery
            {
                AccountType = _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = employerAccountId
            });
            
            var model = (SubscriptionsViewModel)result;
            return View(model);
        }

    }
}