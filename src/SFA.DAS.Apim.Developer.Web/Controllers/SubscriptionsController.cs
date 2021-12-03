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
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAdminAccount))]
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
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAdminAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-renew", Name = RouteNames.EmployerRenewKey)]
        [Route("{ukprn}/subscriptions/{id}/confirm-renew", Name = RouteNames.ProviderRenewKey)]
        public IActionResult ConfirmRenewKey([FromRoute]string employerAccountId, [FromRoute]string id, [FromRoute]int? ukprn)
        {
            return View(_serviceParameters.AuthenticationType == AuthenticationType.Employer);
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAdminAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/create", Name = RouteNames.EmployerCreateKey)]
        [Route("{ukprn}/subscriptions/{id}/create", Name = RouteNames.ProviderCreateKey)]
        public async Task<IActionResult> CreateSubscription([FromRoute]string employerAccountId, [FromRoute]string id, [FromRoute]int? ukprn)
        {
            await _mediator.Send(new CreateSubscriptionKeyCommand
            {
                AccountType =  _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = _serviceParameters.AuthenticationType is AuthenticationType.Employer ? employerAccountId : ukprn.ToString(),
                ProductId = id
            });

            var employerViewSubscription = _serviceParameters.AuthenticationType is AuthenticationType.Employer ? RouteNames.EmployerViewSubscription : RouteNames.ProviderViewSubscription;
            return RedirectToRoute(employerViewSubscription, new { employerAccountId, id, ukprn });
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAdminAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}", Name = RouteNames.EmployerViewSubscription)]
        [Route("{ukprn}/subscriptions/{id}", Name = RouteNames.ProviderViewSubscription)]
        public async Task<IActionResult> ViewProductSubscription([FromRoute]string employerAccountId, [FromRoute]string id,[FromRoute]int? ukprn, [FromQuery]bool? keyRenewed = null)
        {
            var result = await _mediator.Send(new GetSubscriptionQuery
            {
                AccountType =  _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = _serviceParameters.AuthenticationType is AuthenticationType.Employer ? employerAccountId : ukprn.ToString(),
                ProductId = id
            });

            
            var model = new SubscriptionViewModel
            {
                Product = result.Product,
                EmployerAccountId = employerAccountId,
                Ukprn = ukprn,
                ShowRenewedBanner = keyRenewed != null && keyRenewed.Value,
                RenewKeyRouteName = _serviceParameters.AuthenticationType is AuthenticationType.Employer ?RouteNames.EmployerRenewKey : RouteNames.ProviderRenewKey
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyNames.HasProviderOrEmployerAdminAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-renew", Name = RouteNames.EmployerRenewKey)]
        [Route("{ukprn}/subscriptions/{id}/confirm-renew", Name = RouteNames.ProviderRenewKey)]
        public async Task<IActionResult> PostConfirmRenewKey([FromRoute]string employerAccountId, [FromRoute]string id,[FromRoute]int? ukprn, RenewKeyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmRenewKey", _serviceParameters.AuthenticationType is AuthenticationType.Employer);
            }

            var routeName = _serviceParameters.AuthenticationType is AuthenticationType.Employer 
                ? RouteNames.EmployerViewSubscription : RouteNames.ProviderViewSubscription;
            
            if (viewModel.ConfirmRenew.HasValue && viewModel.ConfirmRenew.Value)
            {
                var renewCommand = new RenewSubscriptionKeyCommand
                {
                    AccountIdentifier = _serviceParameters.AuthenticationType is AuthenticationType.Employer ? employerAccountId : ukprn.ToString(),
                    ProductId = id
                };
                await _mediator.Send(renewCommand);
                return RedirectToRoute(routeName, new { employerAccountId, id, ukprn, keyRenewed = true });    
            }
            return RedirectToRoute(routeName, new { employerAccountId, ukprn, id });
        }
    }
}