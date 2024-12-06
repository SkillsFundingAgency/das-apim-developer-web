using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.TagHelpers
{
    public class ApiDescriptionHelper : IApiDescriptionHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ApimDeveloperWeb _configuration;

        public ApiDescriptionHelper (IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, IOptions<ApimDeveloperWeb> configuration)
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _configuration = configuration.Value;
        }

        public string ProcessApiDescription(string data, string keyName, string apiName, bool showDocumentationUrl = true)
        {
            if (ApiDescriptionLookup.Descriptions.ContainsKey(keyName))
            {
                data = ApiDescriptionLookup.Descriptions[keyName];
            }

            if (!showDocumentationUrl)
            {
                return data;
            }
            
            data = data.Replace(".", ".");
            var converted = data;
            
            var helper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            
            var url = helper.RouteUrl(RouteNames.Documentation, new { apiName }, "https", _configuration.DocumentationBaseUrl);
            
            if (!string.IsNullOrEmpty(url))
            {
                converted += $" Give the API key and <a href='{url}' class='govuk-link govuk-link--no-visited-state'>this link to the API page</a> to your developer.";
            }
            
            return converted;
        }
    }

    public static class ApiDescriptionLookup
    {
        public static Dictionary<string, string> Descriptions => new Dictionary<string, string>
        {
            // This overrides the product descriptions drawn from the swagger doc annotations on each products' Program class in das-apim-endpoints.
            // Sandbox environments don't exist in APIM and so their descriptions must be overidden.
            { "VacanciesManageOuterApi-Sandbox", "Test your implementation of the Recruitment API." },
            { "VacanciesOuterApi", "Get and display adverts from Find an apprenticeship." },
            { "TrackProgressOuterApi-Sandbox", "Test your implementation of the Track apprenticeship progress API." }
        };
    }
}