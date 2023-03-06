using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.DeleteSubscriptionKey;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetSubscription;
using SFA.DAS.Apim.Developer.Domain.Employers;
using SFA.DAS.Apim.Developer.Domain.Extensions;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    [Employer.Shared.UI.Attributes.SetNavigationSection(NavigationSection.RecruitHome)]
    [SetNavigationSection(Provider.Shared.UI.NavigationSection.Recruit)]
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
        public IActionResult ApiList([FromQuery] string apiName = null, [FromQuery] bool? keyDeleted = null)
        {
            if(_serviceParameters.AuthenticationType == AuthenticationType.Provider)
            {
                var ukprn = HttpContext.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;
                return new RedirectToRouteResult(RouteNames.ProviderApiHub, new {ukprn, apiName, keyDeleted });
            }
            if(_serviceParameters.AuthenticationType == AuthenticationType.Employer)
            {    
                var employerAccountClaim = HttpContext.User.FindFirst(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
                var accounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(employerAccountClaim);
                return new RedirectToRouteResult(RouteNames.EmployerApiHub, new {employerAccountId = accounts.FirstOrDefault().Key, apiName, keyDeleted });
            }
            var externalId = HttpContext.User.FindFirst(c => c.Type.Equals(ExternalUserClaims.Id)).Value;
            return new RedirectToRouteResult(RouteNames.ExternalApiHub, new { externalId, apiName, keyDeleted });
        }
        
        
        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("accounts/{employerAccountId}/subscriptions", Name = RouteNames.EmployerApiHub)]
        [Route("{ukprn:int}/subscriptions", Name = RouteNames.ProviderApiHub)]
        [Route("{externalId}/subscriptions", Name = RouteNames.ExternalApiHub)]
        public async Task<IActionResult> ApiHub([FromRoute]string employerAccountId, [FromRoute]int? ukprn, [FromRoute]string externalId = null, [FromQuery] string apiName = null, [FromQuery] bool? keyDeleted = null)
        {
            var subscriptionRouteModel = new SubscriptionRouteModel(_serviceParameters, employerAccountId, ukprn, externalId);

            var result = await _mediator.Send(new GetAvailableProductsQuery(
                _serviceParameters.AuthenticationType.GetDescription(),
                subscriptionRouteModel.AccountIdentifier));
            
            var model = (SubscriptionsViewModel)result;
            model.EmployerAccountId = employerAccountId;
            model.Ukprn = ukprn;
            model.ExternalId = externalId;
            model.CreateKeyRouteName = subscriptionRouteModel.CreateKeyRouteName;
            model.ViewKeyRouteName = subscriptionRouteModel.ViewSubscriptionRouteName;
            model.AuthenticationType = _serviceParameters.AuthenticationType;
            model.ShowDeletedBanner = keyDeleted != null && keyDeleted.Value;
            model.ApiName = apiName;
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
                DeleteKeyRouteName = subscriptionRouteModel.DeleteKeyRouteName,
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

        [HttpGet]
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-delete", Name = RouteNames.EmployerDeleteKey)]
        [Route("{ukprn:int}/subscriptions/{id}/confirm-delete", Name = RouteNames.ProviderDeleteKey)]
        [Route("{externalId}/subscriptions/{id}/confirm-delete", Name = RouteNames.ExternalDeleteKey)]
        public IActionResult ConfirmDeleteKey([FromRoute] string employerAccountId, [FromRoute] string id, [FromRoute] int? ukprn, [FromRoute] string externalId)
        {
            return View(_serviceParameters.AuthenticationType);
        }

        [HttpPost]
        [Authorize(Policy = nameof(PolicyNames.HasProviderEmployerAdminOrExternalAccount))]
        [Route("accounts/{employerAccountId}/subscriptions/{id}/confirm-delete", Name = RouteNames.EmployerDeleteKey)]
        [Route("{ukprn:int}/subscriptions/{id}/confirm-delete", Name = RouteNames.ProviderDeleteKey)]
        [Route("{externalId}/subscriptions/{id}/confirm-delete", Name = RouteNames.ExternalDeleteKey)]
        public async Task<IActionResult> PostConfirmDeleteKey([FromRoute] string employerAccountId, [FromRoute] string id, [FromRoute] int? ukprn, [FromRoute] string externalId, DeleteKeyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmDeleteKey", _serviceParameters.AuthenticationType);
            }

            if (viewModel.ConfirmDelete.HasValue && viewModel.ConfirmDelete.Value)
            {
                var subscriptionRouteModel = new SubscriptionRouteModel(_serviceParameters, employerAccountId, ukprn, externalId);

                // Query handler to fetch the API Display name and other properties. 
                var result = await _mediator.Send(new GetSubscriptionQuery
                {
                    AccountType = _serviceParameters.AuthenticationType.GetDescription(),
                    AccountIdentifier = subscriptionRouteModel.AccountIdentifier,
                    ProductId = id
                });

                // Command handler to delete the subscription via Api Client.
                await _mediator.Send(new DeleteSubscriptionKeyCommand
                {
                    AccountIdentifier = subscriptionRouteModel.AccountIdentifier,
                    ProductId = id
                });

                return new RedirectToRouteResult(RouteNames.ApiList, new { apiName = result.Product.DisplayName, keyDeleted = true });
            }

            return new RedirectToRouteResult(RouteNames.ApiList, new { keyDeleted = false });
        }
    }
}