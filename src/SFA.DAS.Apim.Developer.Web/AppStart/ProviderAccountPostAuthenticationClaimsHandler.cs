using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Domain.Employers;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.Apim.Developer.Web.AppStart;

public class ProviderAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    private readonly IEmployerAccountService _accountsSvc;
    private readonly IConfiguration _configuration;
    private readonly ApimDeveloperWeb _apimDeveloperWebConfiguration;

    public ProviderAccountPostAuthenticationClaimsHandler(IEmployerAccountService accountsSvc, IConfiguration configuration, IOptions<ApimDeveloperWeb> apimDeveloperWebConfiguration)
    {
        _accountsSvc = accountsSvc;
        _configuration = configuration;
        _apimDeveloperWebConfiguration = apimDeveloperWebConfiguration.Value;
    }
    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext ctx)
    {
        if (_configuration["StubAuth"] != null && _configuration["StubAuth"]
                .Equals("true", StringComparison.CurrentCultureIgnoreCase))
        {

            
        }

        return new List<Claim> { new Claim("blach", "bah") };
    }
}