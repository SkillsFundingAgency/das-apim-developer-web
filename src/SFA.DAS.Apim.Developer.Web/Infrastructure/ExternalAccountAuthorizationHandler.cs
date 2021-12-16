using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public interface IExternalAccountAuthorizationHandler
    {
        bool IsExternalUserAuthorised(AuthorizationHandlerContext context);
    }
    public class ExternalAccountAuthorizationHandler : AuthorizationHandler<ExternalAccountRequirement>, IExternalAccountAuthorizationHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExternalAccountAuthorizationHandler (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExternalAccountRequirement requirement)
        {
            if (!IsExternalUserAuthorised(context))
            {
                context.Fail();
                return Task.CompletedTask;
            }
                
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
        
        public bool IsExternalUserAuthorised(AuthorizationHandlerContext context)
        {
            if (!context.User.HasClaim(c => c.Type.Equals(ExternalUserClaims.Id)))
            {
                return false;
            }
            
            if (_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValues.ExternalId))
            {
                var externalIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValues.ExternalId].ToString();
                var externalId = context.User.FindFirst(c => c.Type.Equals(ExternalUserClaims.Id)).Value;

                return externalId.Equals(externalIdFromUrl);    
            }

            return false;
        }
    }
}