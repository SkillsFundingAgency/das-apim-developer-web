using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.Apim.Developer.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.Apim.Developer.Web.AcceptanceTests.Steps
{
    [Binding]
    public sealed class ContentSteps
    {
        private readonly ScenarioContext _context;

        public ContentSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("the page content includes the following: (.*)")]
        public async Task ThenThePageContentIncludesTheFollowing(string expectedContent)
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

            var actualContent = await response.Content.ReadAsStringAsync();

            var contains = actualContent.Contains(expectedContent);
            contains.Should().BeTrue();
        }

        [Then("the page content does not include the following: (.*)")]
        public async Task ThenThePageContentNotIncludeTheFollowing(string expectedContent)
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

            var actualContent = await response.Content.ReadAsStringAsync();

            actualContent.Should().NotContain(expectedContent);
        }
    }
}