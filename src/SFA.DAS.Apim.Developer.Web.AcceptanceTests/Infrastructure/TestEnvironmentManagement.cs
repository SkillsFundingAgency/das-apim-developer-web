using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using SFA.DAS.Apim.Developer.MockServer;
using TechTalk.SpecFlow;
using WireMock.Server;

namespace SFA.DAS.Apim.Developer.Web.AcceptanceTests.Infrastructure
{
    [Binding]
    public sealed class TestEnvironmentManagement
    {
        private readonly ScenarioContext _context;
        private static HttpClient _staticClient;
        private static IWireMockServer _staticApiServer;
        private static TestServer _server;
        private CustomWebApplicationFactory<Startup> _webApp;

        public TestEnvironmentManagement(ScenarioContext context)
        {
            _context = context;
        }

        [BeforeScenario("WireMockServer", "Provider")]
        public void StartWebApp()
        {
            _staticApiServer = MockApiServer.Start();
            _webApp = new CustomWebApplicationFactory<Startup>("Provider");
            
            _server = _webApp.Server;

            _staticClient = _server.CreateClient();
            _context.Set(_server, ContextKeys.TestServer);
            _context.Set(_staticClient,ContextKeys.HttpClient);
        }
        
        
        [AfterScenario("WireMockServer")]
        public void StopEnvironment()
        {
            
            _webApp.Dispose();
            _server.Dispose();
            
            _staticApiServer?.Stop();
            _staticApiServer?.Dispose();
            _staticClient?.Dispose();
            
        }
    }
}