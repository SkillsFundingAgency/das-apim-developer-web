using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Web.TagHelpers
{
    [HtmlTargetElement("api-description")]
    public class ApiDescriptionTagHelper : TagHelper
    {
        private readonly IApiDescriptionHelper _apiDescriptionHelper;

        public ApiDescriptionTagHelper (IApiDescriptionHelper apiDescriptionHelper)
        {
            _apiDescriptionHelper = apiDescriptionHelper;
        }
        public string Data { get; set; }
        public string KeyName { get; set; }
        public bool ShowDocumentationUrl { get; set; } = true;
        public string TagName { get; set; }
        public string Class { get; set; }
        public override void Process(TagHelperContext tagHelperContext, TagHelperOutput tagHelperOutput)
        {
            
            tagHelperOutput.TagName = TagName;
            tagHelperOutput.AddClass(Class,HtmlEncoder.Default);
            tagHelperOutput.Content.SetHtmlContent(_apiDescriptionHelper.ProcessApiDescription(Data, KeyName, ShowDocumentationUrl));
            tagHelperOutput.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}