using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiClient _apiClient;

        public UserService (IHttpContextAccessor httpContextAccessor, IApiClient apiClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _apiClient = apiClient;
        }
        public async Task<UserDetails> AuthenticateUser(string email, string password)
        {
            var authenticateResult =
                await _apiClient.Post<PostAuthenticateUserResponse>(new PostAuthenticateUserRequest(email, password));

            if (authenticateResult.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }
            
            var userDetails = (UserDetails)authenticateResult.Body.User;

            if (!userDetails.Authenticated)
            {
                return userDetails;
            }
            
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ExternalUserClaims.Id, userDetails.Id),
                new Claim(ClaimTypes.Name, $"{userDetails.FirstName} {userDetails.LastName}")
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            
            return userDetails;
        }
    }
}