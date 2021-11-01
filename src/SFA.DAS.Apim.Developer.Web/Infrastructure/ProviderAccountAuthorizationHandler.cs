using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{   
    public class ProviderAccountAuthorizationHandler : AuthorizationHandler<ProviderAccountRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProviderAccountAuthorizationHandler (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProviderAccountRequirement requirement)
        {
            if (!IsProviderAuthorised(context))
            {
                context.Fail();
                return Task.CompletedTask;
            }
                
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
        
        private bool IsProviderAuthorised(AuthorizationHandlerContext context)
        {
            if (!context.User.HasClaim(c => c.Type.Equals(ProviderClaims.ProviderUkprn)))
            {
                return false;
            }
            
            if (_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValues.Ukprn))
            {
                var ukPrnFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValues.Ukprn].ToString();
                var ukPrn = context.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;

                return ukPrn.Equals(ukPrnFromUrl);    
            }

            return false;
        }
    }
}