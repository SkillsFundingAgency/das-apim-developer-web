using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.Employers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public class EmployerStubAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        
        public EmployerStubAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var accountClaims = new Dictionary<string, EmployerUserAccountItem>();
            accountClaims.Add("", new EmployerUserAccountItem
            {
                Role = "Owner",
                AccountId = "ABC123",
                EmployerName = "Stub Employer"
            });
            
            var claims = new[]
            {
                new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(accountClaims)),
                new Claim(EmployerClaims.EmployerEmailClaimsTypeIdentifier, "testemployer@user.com"),
                new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };
            var identity = new ClaimsIdentity(claims, "Employer-stub");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Employer-stub");
 
            var result = AuthenticateResult.Success(ticket);
 
            return Task.FromResult(result);
        }
    }
}