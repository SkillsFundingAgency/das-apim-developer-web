using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.TagHelpers
{
    public class ApiDescriptionHelper : IApiDescriptionHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        public ApiDescriptionHelper (IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
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
            var url = helper.RouteUrl(RouteNames.Documentation, new {apiName = apiName});
            
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
            { "VacanciesManageOuterApi-Sandbox", "Test creating an advert on Find an apprenticeship using your existing systems." }
        };
    }
}