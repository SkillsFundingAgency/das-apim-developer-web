using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Services;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Services
{
    public class WhenAuthenticatingUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_To_Authenticate_And_If_Valid_Context_Created(
            string email, 
            string password,
            PostAuthenticateUserResponse apiResponse,
            [Frozen] Mock<IServiceProvider> serviceProvider,
            [Frozen] Mock<IAuthenticationService> authenticationService,
            [Frozen] Mock<IApiClient> apiClient,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            UserService userService)
        {
            //Arrange
            apiResponse.User.Authenticated = true;
            serviceProvider.Setup(x => x.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationService.Object);
            apiClient.Setup(x => x.Post<PostAuthenticateUserResponse>(It.Is<PostAuthenticateUserRequest>(c =>
                    ((PostAuthenticateUserRequestData)c.Data).Email.Equals(email))))
                .ReturnsAsync(new ApiResponse<PostAuthenticateUserResponse>(apiResponse, HttpStatusCode.OK, ""));
            
            //Act
            var actual = await userService.AuthenticateUser(email, password);
            
            //Assert
            actual.Should().BeEquivalentTo(apiResponse.User);
            authenticationService.Verify(x=>x.SignInAsync(It.IsAny<HttpContext>(), CookieAuthenticationDefaults.AuthenticationScheme, 
                It.Is<ClaimsPrincipal>(c=>c.HasClaim(ExternalUserClaims.Id,apiResponse.User.Id)), It.IsAny<AuthenticationProperties>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Request_Is_Valid_And_Not_Authenticated_Then_Null_Returned_And_Not_Signed_In(
            string email, 
            string password,
            PostAuthenticateUserResponse apiResponse,
            [Frozen] Mock<IServiceProvider> serviceProvider,
            [Frozen] Mock<IAuthenticationService> authenticationService,
            [Frozen] Mock<IApiClient> apiClient,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            UserService userService)
        {
            //Arrange
            apiResponse.User.Authenticated = false;
            serviceProvider.Setup(x => x.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationService.Object);
            apiClient.Setup(x => x.Post<PostAuthenticateUserResponse>(It.Is<PostAuthenticateUserRequest>(c =>
                    ((PostAuthenticateUserRequestData)c.Data).Email.Equals(email))))
                .ReturnsAsync(new ApiResponse<PostAuthenticateUserResponse>(apiResponse, HttpStatusCode.OK, ""));
            
            //Act
            var actual = await userService.AuthenticateUser(email, password);
            
            //Assert
            actual.Should().BeNull();
            authenticationService.Verify(x=>x.SignInAsync(It.IsAny<HttpContext>(), CookieAuthenticationDefaults.AuthenticationScheme, 
                It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Never);
        }
    }
}