using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.Employers;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.Apim.Developer.Web.AppStart;

public class EmployerAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    private readonly IEmployerAccountService _accountsSvc;
    private readonly IConfiguration _configuration;

    public EmployerAccountPostAuthenticationClaimsHandler(IEmployerAccountService accountsSvc, IConfiguration configuration)
    {
        _accountsSvc = accountsSvc;
        _configuration = configuration;
    }
    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext ctx)
    {
        if (_configuration["LocalStubAuth"] != null && _configuration["LocalStubAuth"]
                .Equals("true", StringComparison.CurrentCultureIgnoreCase))
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
                new Claim(EmployerClaims.EmployerEmailClaimsTypeIdentifier, _configuration["NoAuthEmail"]),
                new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, Guid.NewGuid().ToString())
            };
            
            return claims.ToList();
        }

        var userId = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
            .Value;
        var email = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.Email))
            .Value;
        

        var returnClaims = new List<Claim>();
        var result = await _accountsSvc.GetUserAccounts(userId, email);

        if (result.IsSuspended)
        {
            returnClaims.Add(new Claim(ClaimTypes.AuthorizationDecision,"Suspended"));
        }

        var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
        returnClaims.Add(new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json));
        return returnClaims;
    }
}