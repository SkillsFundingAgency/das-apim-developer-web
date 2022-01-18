using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.Subscriptions;
using SFA.DAS.Apim.Developer.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.Apim.Developer.Web.AcceptanceTests.Steps
{
    [Binding]
    public class SubscriptionKeySteps
    {
        private readonly ScenarioContext _context;

        public SubscriptionKeySteps (ScenarioContext context)
        {
            _context = context;
        }
        
        [Then("there is a row for each product subscription")]
        public async Task ThenThereIsARowForEachProductSubscription()
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

            var actualContent = await response.Content.ReadAsStringAsync();

            var json = DataFileManager.GetFile("product-subscriptions.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<ProductSubscriptions>(json);

            foreach (var apiProduct in expectedApiResponse.Products)
            {
                actualContent.Should().Contain(HttpUtility.HtmlEncode(apiProduct.DisplayName));
            }
        }

        [Then("the (.*) link is shown")]
        public async Task ThenTheExistingSubscriptionsShowViewKey(string subscription)
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

            var actualContent = await response.Content.ReadAsStringAsync();

            var json = DataFileManager.GetFile("product-subscriptions.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<ProductSubscriptions>(json);

            var expected = subscription.Equals("subscribed") ? "View" : "Get";
            
            foreach (var apiProduct in expectedApiResponse.Products
                         .Where(c=> subscription.Equals("subscribed") ? 
                             !string.IsNullOrEmpty(c.Key) : string.IsNullOrEmpty(c.Key))
                     )
            {
                actualContent.Should().Contain($@"{expected} key <span class=""govuk-visually-hidden"">for the {HttpUtility.HtmlEncode(apiProduct.DisplayName)}</span>");
            }
        }
    }
}