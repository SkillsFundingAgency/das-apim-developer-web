using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public class ProviderOrEmployerAccountAuthorizationHandler : AuthorizationHandler<ProviderOrEmployerAccountRequirement>
    {
        private readonly IProviderAccountAuthorisationHandler _providerAccountAuthorisationHandler;
        private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorisationHandler;

        public ProviderOrEmployerAccountAuthorizationHandler (IProviderAccountAuthorisationHandler providerAccountAuthorisationHandler, IEmployerAccountAuthorisationHandler employerAccountAuthorisationHandler)
        {
            _providerAccountAuthorisationHandler = providerAccountAuthorisationHandler;
            _employerAccountAuthorisationHandler = employerAccountAuthorisationHandler;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProviderOrEmployerAccountRequirement requirement)
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