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
        [Route("accounts/{employerAccountId}/subscriptions", Name = RouteNames.EmployerApiHub)]
        public async Task<IActionResult> ApiHub(string employerAccountId)
        {
            var result = await _mediator.Send(new GetAvailableProductsQuery
            {
                AccountType = _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = employerAccountId
            });
            
            var model = (SubscriptionsViewModel)result;
            model.EmployerAccountId = employerAccountId;
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-renew", Name = RouteNames.EmployerRenewKey)]
        public IActionResult ConfirmRenewKey(string employerAccountId, string id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-renew", Name = RouteNames.EmployerRenewKey)]
        public IActionResult PostConfirmRenewKey(string employerAccountId, string id, RenewKeyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmRenewKey", viewModel);
            }

            if (viewModel.ConfirmRenew.HasValue && viewModel.ConfirmRenew.Value)
            {
                return RedirectToRoute(RouteNames.EmployerKeyRenewed, new { employerAccountId });    
            }
            return RedirectToRoute(RouteNames.EmployerApiHub, new { employerAccountId });
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/key-renewed", Name = RouteNames.EmployerKeyRenewed)]
        public IActionResult KeyRenewed(string employerAccountId)
        {
            return View("KeyRenewed", employerAccountId);
        }

    }
}