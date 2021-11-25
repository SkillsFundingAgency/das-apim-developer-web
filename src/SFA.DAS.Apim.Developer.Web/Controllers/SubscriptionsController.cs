using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetSubscription;
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
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAccount))]
        [Route("accounts/{employerAccountId}/subscriptions", Name = RouteNames.EmployerApiHub)]
        [Route("{ukprn}/subscriptions", Name = RouteNames.ProviderApiHub)]
        public async Task<IActionResult> ApiHub([FromRoute]string employerAccountId, [FromRoute]int? ukprn)
        {
            var result = await _mediator.Send(new GetAvailableProductsQuery
            {
                AccountType = _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = _serviceParameters.AuthenticationType == AuthenticationType.Employer ? 
                    employerAccountId : ukprn.ToString()
            });
            
            var model = (SubscriptionsViewModel)result;
            model.EmployerAccountId = employerAccountId;
            model.Ukprn = ukprn;
            model.CreateKeyRouteName = _serviceParameters.AuthenticationType == AuthenticationType.Employer
                ? RouteNames.EmployerCreateKey
                : RouteNames.ProviderCreateKey;
            model.ViewKeyRouteName = _serviceParameters.AuthenticationType == AuthenticationType.Employer
                ? RouteNames.EmployerViewSubscription
                : RouteNames.ProviderViewSubscription;
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-renew", Name = RouteNames.EmployerRenewKey)]
        public IActionResult ConfirmRenewKey(string employerAccountId, string id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/create", Name = RouteNames.EmployerCreateKey)]
        public async Task<IActionResult> CreateSubscription(string employerAccountId, string id)
        {
            await _mediator.Send(new CreateSubscriptionKeyCommand
            {
                AccountType =  _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = employerAccountId,
                ProductId = id
            });

            return RedirectToRoute(RouteNames.EmployerViewSubscription, new { employerAccountId, id });
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}", Name = RouteNames.EmployerViewSubscription)]
        public async Task<IActionResult> ViewProductSubscription(string employerAccountId, string id, [FromQuery]bool? keyRenewed = null)
        {
            var result = await _mediator.Send(new GetSubscriptionQuery
            {
                AccountType =  _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = employerAccountId,
                ProductId = id
            });

            
            var model = new SubscriptionViewModel
            {
                Product = result.Product,
                EmployerAccountId = employerAccountId,
                ShowRenewedBanner = keyRenewed != null && keyRenewed.Value
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-renew", Name = RouteNames.EmployerRenewKey)]
        public async Task<IActionResult> PostConfirmRenewKey(string employerAccountId, string id, RenewKeyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmRenewKey", viewModel);
            }

            if (viewModel.ConfirmRenew.HasValue && viewModel.ConfirmRenew.Value)
            {
                var renewCommand = new RenewSubscriptionKeyCommand
                {
                    AccountIdentifier = employerAccountId,
                    ProductId = id
                };
                await _mediator.Send(renewCommand);
                return RedirectToRoute(RouteNames.EmployerViewSubscription, new { employerAccountId, id, keyRenewed = true });    
            }
            return RedirectToRoute(RouteNames.EmployerViewSubscription, new { employerAccountId, id });
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