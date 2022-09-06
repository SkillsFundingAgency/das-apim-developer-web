﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.Extensions;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApimDeveloperWeb _configuration;

        public HomeController (IMediator mediator, IOptions<ApimDeveloperWeb> configuration)
        {
            _mediator = mediator;
            _configuration = configuration.Value;
        }
            
        [HttpGet]
        [Route("",Name = RouteNames.Index)]

        public async Task <IActionResult> Index()
        {
            var (apiProducts, externalProducts) = await TaskEx.AwaitAll(
                _mediator.Send(new GetAvailableProductsQuery("Documentation")),
                _mediator.Send(new GetAvailableProductsQuery("ExternalUsers")));

            var model = new HomePageViewModel
            {
                ApiProducts = apiProducts.Products.Products.Select(c=>(SubscriptionItem)c).ToList(),
                ExternalProducts = externalProducts.Products.Products.Select(c => (SubscriptionItem)c).ToList(),
                DocumentationBaseUrl = _configuration.DocumentationBaseUrl
            };
            return View(model);
        }
    }
}