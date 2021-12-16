using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetSubscription;
using SFA.DAS.Apim.Developer.Domain.Employers;
using SFA.DAS.Apim.Developer.Domain.Extensions;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;
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
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("subscriptions/api-list", Name = RouteNames.ApiList)]
        public IActionResult ApiList()
        {
            if(_serviceParameters.AuthenticationType == AuthenticationType.Provider)
            {
                var ukprn = HttpContext.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;
                return new RedirectToRouteResult(RouteNames.ProviderApiHub, new {ukprn});
            }
            if(_serviceParameters.AuthenticationType == AuthenticationType.Employer)
            {    
                var employerAccountClaim = HttpContext.User.FindFirst(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
                var accounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(employerAccountClaim);
                return new RedirectToRouteResult(RouteNames.EmployerApiHub, new {employerAccountId = accounts.FirstOrDefault().Key});
            }
            var externalId = HttpContext.User.FindFirst(c => c.Type.Equals(ExternalUserClaims.Id)).Value;
            return new RedirectToRouteResult(RouteNames.ExternalApiHub, new {externalId});
        }
        
        
        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("accounts/{employerAccountId}/subscriptions", Name = RouteNames.EmployerApiHub)]
        [Route("{ukprn:int}/subscriptions", Name = RouteNames.ProviderApiHub)]
        [Route("{externalId}/subscriptions", Name = RouteNames.ExternalApiHub)]
        public async Task<IActionResult> ApiHub([FromRoute]string employerAccountId, [FromRoute]int? ukprn, [FromRoute]string externalId = null)
        {
            var subscriptionRouteModel = new SubscriptionRouteModel(_serviceParameters, employerAccountId, ukprn, externalId);
            
            var result = await _mediator.Send(new GetAvailableProductsQuery
            {
                AccountType = _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = subscriptionRouteModel.AccountIdentifier
            });
            
            var model = (SubscriptionsViewModel)result;
            model.EmployerAccountId = employerAccountId;
            model.Ukprn = ukprn;
            model.ExternalId = externalId;
            model.CreateKeyRouteName = subscriptionRouteModel.CreateKeyRouteName;
            model.ViewKeyRouteName = subscriptionRouteModel.ViewSubscriptionRouteName;
            model.AuthenticationType = _serviceParameters.AuthenticationType;
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-renew", Name = RouteNames.EmployerRenewKey)]
        [Route("{ukprn:int}/subscriptions/{id}/confirm-renew", Name = RouteNames.ProviderRenewKey)]
        [Route("{externalId}/subscriptions/{id}/confirm-renew", Name = RouteNames.ExternalRenewKey)]
        public IActionResult ConfirmRenewKey([FromRoute]string employerAccountId, [FromRoute]string id, [FromRoute]int? ukprn, [FromRoute]string externalId)
        {
            return View(_serviceParameters.AuthenticationType);
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/create", Name = RouteNames.EmployerCreateKey)]
        [Route("{ukprn:int}/subscriptions/{id}/create", Name = RouteNames.ProviderCreateKey)]
        [Route("{externalId}/subscriptions/{id}/create", Name = RouteNames.ExternalCreateKey)]
        public async Task<IActionResult> CreateSubscription([FromRoute]string employerAccountId, [FromRoute]string id, [FromRoute]int? ukprn, [FromRoute]string externalId)
        {
            var subscriptionRouteModel = new SubscriptionRouteModel(_serviceParameters, employerAccountId, ukprn, externalId);
            
            await _mediator.Send(new CreateSubscriptionKeyCommand
            {
                AccountType =  _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = subscriptionRouteModel.AccountIdentifier,
                ProductId = id
            });

            return RedirectToRoute(subscriptionRouteModel.ViewSubscriptionRouteName, new { employerAccountId, id, ukprn, externalId });
        }

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}", Name = RouteNames.EmployerViewSubscription)]
        [Route("{ukprn:int}/subscriptions/{id}", Name = RouteNames.ProviderViewSubscription)]
        [Route("{externalId}/subscriptions/{id}", Name = RouteNames.ExternalViewSubscription)]
        public async Task<IActionResult> ViewProductSubscription([FromRoute]string employerAccountId, [FromRoute]string id,[FromRoute]int? ukprn, [FromQuery]bool? keyRenewed = null, [FromRoute]string externalId = null)
        {
            var subscriptionRouteModel = new SubscriptionRouteModel(_serviceParameters, employerAccountId, ukprn, externalId);
            
            var result = await _mediator.Send(new GetSubscriptionQuery
            {
                AccountType =  _serviceParameters.AuthenticationType.GetDescription(),
                AccountIdentifier = subscriptionRouteModel.AccountIdentifier,
                ProductId = id
            });

            
            var model = new SubscriptionViewModel
            {
                Product = result.Product,
                EmployerAccountId = employerAccountId,
                Ukprn = ukprn,
                ExternalId = externalId,
                ShowRenewedBanner = keyRenewed != null && keyRenewed.Value,
                RenewKeyRouteName = subscriptionRouteModel.RenewKeyRouteName,
                AuthenticationType = _serviceParameters.AuthenticationType
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-renew", Name = RouteNames.EmployerRenewKey)]
        [Route("{ukprn:int}/subscriptions/{id}/confirm-renew", Name = RouteNames.ProviderRenewKey)]
        [Route("{externalId}/subscriptions/{id}/confirm-renew", Name = RouteNames.ExternalRenewKey)]
        public async Task<IActionResult> PostConfirmRenewKey([FromRoute]string employerAccountId, [FromRoute]string id,[FromRoute]int? ukprn,[FromRoute]string externalId, RenewKeyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmRenewKey", _serviceParameters.AuthenticationType);
            }
            
            var subscriptionRouteModel = new SubscriptionRouteModel(_serviceParameters, employerAccountId, ukprn, externalId);
            
            if (viewModel.ConfirmRenew.HasValue && viewModel.ConfirmRenew.Value)
            {
                var renewCommand = new RenewSubscriptionKeyCommand
                {
                    AccountIdentifier = subscriptionRouteModel.AccountIdentifier,
                    ProductId = id
                };
                await _mediator.Send(renewCommand);
                return RedirectToRoute(subscriptionRouteModel.ViewSubscriptionRouteName, new { employerAccountId, id, ukprn, keyRenewed = true, externalId });    
            }
            return RedirectToRoute(subscriptionRouteModel.ViewSubscriptionRouteName, new { employerAccountId, ukprn, id, externalId });
        }
    }
}