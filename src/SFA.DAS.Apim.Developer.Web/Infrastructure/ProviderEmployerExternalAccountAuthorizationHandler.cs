using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public class ProviderEmployerExternalAccountAuthorizationHandler : AuthorizationHandler<ProviderEmployerExternalAccountRequirement>
    {
        private readonly IProviderAccountAuthorisationHandler _providerAccountAuthorisationHandler;
        private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorisationHandler;
        private readonly IExternalAccountAuthorizationHandler _externalAccountAuthorizationHandler;

        public ProviderEmployerExternalAccountAuthorizationHandler (IProviderAccountAuthorisationHandler providerAccountAuthorisationHandler, IEmployerAccountAuthorisationHandler employerAccountAuthorisationHandler, IExternalAccountAuthorizationHandler externalAccountAuthorizationHandler)
        {
            _providerAccountAuthorisationHandler = providerAccountAuthorisationHandler;
            _employerAccountAuthorisationHandler = employerAccountAuthorisationHandler;
            _externalAccountAuthorizationHandler = externalAccountAuthorizationHandler;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProviderEmployerExternalAccountRequirement requirement)
        {
            var result = false;
            if (context.User.HasClaim(c => c.Type.Equals(ProviderClaims.ProviderUkprn)))
            {
                result = _providerAccountAuthorisationHandler.IsProviderAuthorised(context);
            }
            else if (context.User.HasClaim(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)))
            {
                result = _employerAccountAuthorisationHandler.IsEmployerAuthorised(context, false);
            }
            else if (context.User.HasClaim(c => c.Type.Equals(ExternalUserClaims.Id)))
            {
                result = _externalAccountAuthorizationHandler.IsExternalUserAuthorised(context);
            }
            
            if (result)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}