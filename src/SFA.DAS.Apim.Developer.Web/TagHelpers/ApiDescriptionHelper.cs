using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

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

        public string ProcessApiDescription(string data)
        {
            data = data.Replace(".", ".<br>");
            var converted = data;
            var startLinkIndex = data.IndexOf("{{", StringComparison.Ordinal);
            var endLinkIndex = data.IndexOf("}}", StringComparison.Ordinal);

            if (startLinkIndex != -1)
            {
                var linkData = data.Substring(startLinkIndex + 2, endLinkIndex - startLinkIndex - 2);

                var helper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            
                var url = helper.RouteUrl(linkData.Split("|")[1]);
            
                var anchor = $" href='{url}' class='govuk-link govuk-link--no-visited-state'>{linkData.Split("|")[0]}";

                var stringTest = data.Substring(0, startLinkIndex);
                var stringTest1 = data.Substring(endLinkIndex + 2, data.Length-endLinkIndex - 2);

                converted = stringTest + "<a" + anchor + "</a>" + stringTest1;
            }
            
            return converted;
        }
    }
}