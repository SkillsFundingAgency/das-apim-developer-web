using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Application.Employer.Services;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.Api;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using System.Security.Claims;
using SFA.DAS.DfESignIn.Auth.Interfaces;


namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public class CustomClaims : ICustomClaims
    {
        public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
        {
            var value = tokenValidatedContext?.Principal?.Identities.First().Claims
                .FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                ?.Value;

            var ukPrn = 10000001;

            List<Claim> claims = List<Claim>();

            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, ukPrn.ToString()));
            claims.Add(new Claim("http://schemas.portal.com/displayname", "Display Name"));
            claims.Add(new Claim("http://schemas.portal.com/ukprn", ukPrn.ToString()));


            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, "10000001"));
            claims.Add(new Claim(ProviderClaims.DisplayName, "APIM Provider User"));
            claims.Add(new Claim(ProviderClaims.Service, "DAA"));
            claims.Add(new Claim(ProviderClaims.ProviderUkprn, "10000001"));

            //HttpContext.Items.Add(ClaimsIdentity.DefaultNameClaimType, "10000001");
            //HttpContext.Items.Add(ProviderClaims.DisplayName, "APIM Provider User");

            return claims;
        }

        private List<T> List<T>()
        {
            throw new NotImplementedException();
        }
    }
}