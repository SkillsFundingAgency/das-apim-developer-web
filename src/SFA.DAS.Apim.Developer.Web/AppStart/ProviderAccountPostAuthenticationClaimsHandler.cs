using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.GovUK.Auth.Services;
using System.Security.Claims;

namespace SFA.DAS.Apim.Developer.Web.AppStart;

public class ProviderAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    private readonly IConfiguration _configuration;


    public Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
    {
        throw new NotImplementedException();
    }
}