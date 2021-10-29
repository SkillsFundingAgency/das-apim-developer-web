using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.Apim.Developer.Domain.Employers;
using SFA.DAS.Apim.Developer.Domain.Employers.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    public interface IEmployerAccountAuthorisationHandler
    {
        bool IsEmployerAuthorised(AuthorizationHandlerContext context, bool allowAllUserRoles);
    }

    public class EmployerAccountAuthorizationHandler: AuthorizationHandler<EmployerAccountRequirement>, IEmployerAccountAuthorisationHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployerAccountService _accountsService;
        private readonly ILogger<EmployerAccountAuthorizationHandler> _logger;

        public EmployerAccountAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IEmployerAccountService accountsService, ILogger<EmployerAccountAuthorizationHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountsService = accountsService;
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountRequirement requirement)
        {
            if (!IsEmployerAuthorised(context, false))
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        public bool IsEmployerAuthorised(AuthorizationHandlerContext context, bool allowAllUserRoles)
        {
            if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValues.EmployerAccountId))
            {
                return false;
            }
            var accountIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValues.EmployerAccountId].ToString().ToUpper();
            var employerAccountClaim = context.User.FindFirst(c=>c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));

            if(employerAccountClaim?.Value == null)
                return false;

            Dictionary<string, GetEmployerUserAccountItem> employerAccounts;

            try
            {
                employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, GetEmployerUserAccountItem>>(employerAccountClaim.Value);
            }
            catch (JsonSerializationException e)
            {
                _logger.LogError(e, "Could not deserialize employer account claim for user", employerAccountClaim.Value);
                return false;
            }

            GetEmployerUserAccountItem employerIdentifier = null;

            if (employerAccounts != null)
            {
                employerIdentifier = employerAccounts.ContainsKey(accountIdFromUrl) 
                    ? employerAccounts[accountIdFromUrl] : null;
            }

            if (employerAccounts == null || !employerAccounts.ContainsKey(accountIdFromUrl))
            {
                if (!context.User.HasClaim(c => c.Type.Equals(EmployerClaims.IdamsUserIdClaimTypeIdentifier)))
                    return false;

                var userClaim = context.User.Claims
                    .First(c => c.Type.Equals(EmployerClaims.IdamsUserIdClaimTypeIdentifier));

                var userId = userClaim.Value;

                var result = _accountsService.GetUserAccounts(userId).Result;
                
                var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
                var associatedAccountsClaim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);
                
                var updatedEmployerAccounts = JsonConvert.DeserializeObject<Dictionary<string, GetEmployerUserAccountItem>>(associatedAccountsClaim.Value);

                userClaim.Subject.AddClaim(associatedAccountsClaim);
                
                if (!updatedEmployerAccounts.ContainsKey(accountIdFromUrl))
                {
                    return false;
                }

                employerIdentifier = updatedEmployerAccounts[accountIdFromUrl];
            }

            if (!_httpContextAccessor.HttpContext.Items.ContainsKey(ContextItemKeys.EmployerIdentifier))
            {
                _httpContextAccessor.HttpContext.Items.Add(ContextItemKeys.EmployerIdentifier, employerAccounts.GetValueOrDefault(accountIdFromUrl));
            }

            if (!CheckUserRoleForAccess(employerIdentifier, allowAllUserRoles))
            {
                return false;
            }
            
            return true;
        }

        private static bool CheckUserRoleForAccess(GetEmployerUserAccountItem employerIdentifier, bool allowAllUserRoles)
        {
            if (!Enum.TryParse<EmployerUserRole>(employerIdentifier.Role, true, out var userRole))
            {
                return false;
            }

            return allowAllUserRoles || (userRole == EmployerUserRole.Owner || userRole == EmployerUserRole.Transactor);
        }
    }
}