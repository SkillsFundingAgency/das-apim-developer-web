using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public class ExternalStubAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ExternalStubAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ExternalUserClaims.Id, "384a56e3-14f9-4133-80b2-e10572890f3d")
            };
            var identity = new ClaimsIdentity(claims, "External-stub");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "External-stub");
 
            var result = AuthenticateResult.Success(ticket);
 
            return Task.FromResult(result);
        }
    }
}