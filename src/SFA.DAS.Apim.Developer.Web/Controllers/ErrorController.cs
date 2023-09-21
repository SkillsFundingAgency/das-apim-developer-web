using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Infrastructure.Configuration;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Provider.Shared.UI.Models;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        private readonly ProviderSharedUIConfiguration _providerConfiguration;
        private readonly ServiceParameters _serviceParameters;
        private readonly ExternalLinksConfiguration _configuration;
        private readonly ApimDeveloperWeb _apimDeveloper;
        private readonly IConfiguration _config;

        public ErrorController (IOptions<ExternalLinksConfiguration> configuration, ProviderSharedUIConfiguration providerConfiguration, ServiceParameters serviceParameters, ApimDeveloperWeb apimDeveloper, IConfiguration config)
        {
            _providerConfiguration = providerConfiguration;
            _serviceParameters = serviceParameters;
            _apimDeveloper = apimDeveloper;
            _config = config;
            _configuration = configuration.Value;
        }
        
        [Route("403", Name = RouteNames.Error403)]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied", new Error403ViewModel(_config["ResourceEnvironmentName"])
            {
                DashboardUrl = _serviceParameters.AuthenticationType == AuthenticationType.Employer ? _configuration.ManageApprenticeshipSiteUrl : _providerConfiguration.DashboardUrl + "/account",
                UseDfESignIn = _apimDeveloper.UseDfESignIn
            });
        }
        
        [Route("404", Name = RouteNames.Error404)]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("500", Name = RouteNames.Error500)]
        public IActionResult Error()
        {
            return View();
        }
    }
}