using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.TagHelpers
{
    public class ApiDescriptionHelper(
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IOptions<ApimDeveloperWeb> configuration)
        : IApiDescriptionHelper
    {
        private readonly ApimDeveloperWeb _configuration = configuration.Value;

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
            
            var helper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            
            var url = helper.RouteUrl(RouteNames.Documentation, new { apiName }, "https", _configuration.DocumentationBaseUrl);
            
            if (!string.IsNullOrEmpty(url))
            {
                converted = converted
                    .Replace("{url}", url) // This is the link to the API documentation page.
                    .Replace("v2", "v1");  // This is a temporary fix to ensure that the v1 documentation link is used in the description.
            }

            return converted;
        }
    }

    public static class ApiDescriptionLookup
    {
        public static Dictionary<string, string> Descriptions => new()
        {
            // This overrides the product descriptions drawn from the swagger doc annotations on each products' Program class in das-apim-endpoints.
            // Sandbox environments don't exist in APIM and so their descriptions must be overridden.
            { "VacanciesManageOuterApi-Sandbox", "Test your implementation of the Recruitment API." },
            {
                "VacanciesOuterApi",
                "Get and display adverts from Find an apprenticeship. <div class='govuk-inset-text'>" +
                "<p>The new API version (version 2) lets you display apprenticeships that are available in more than one location.</p>" +
                "<p>You must update to this new version by 1 April 2026. Read more about changing the version you use in the Versioning section of this page.</p>" +
                "<p>If you need to check your current implementation, you can view the old documentation for " +
                "<a class='govuk-link' href='{url}'>Display advert API (version 1)</a></p></div>"
            },
            { "TrackProgressOuterApi-Sandbox", "Test your implementation of the Track apprenticeship progress API." },
            { "TrackProgressOuterApi", "Share data on the progress of your apprenticeships." }
        };
    }
}