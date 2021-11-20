using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public class EmployerViewerAuthorizationHandler : AuthorizationHandler<EmployerViewerRoleRequirement>
    {
       
        private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorizationHandler;
        
        public EmployerViewerAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler)
        {
            _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerViewerRoleRequirement employerViewRoleRequirement)
        {
            
            if (!_employerAccountAuthorizationHandler.IsEmployerAuthorised(context, true))
            {
                return Task.CompletedTask;
            }
            
            context.Succeed(employerViewRoleRequirement);

            return Task.CompletedTask;
        }
    }
}