using System;
using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Infrastructure
{
    public class WhenHandlingExternalAccountAuthorization
    {
        [Test, MoqAutoData]
        public void Then_Returns_True_If_External_User_Is_Authorized(
            Guid id,
            ExternalAccountRequirement requirement,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ExternalAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ExternalUserClaims.Id, id.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.ExternalId, id);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public void Then_False_If_The_Claim_Value_Does_Not_Match_The_Url(
            Guid id,
            Guid routeId,
            ExternalAccountRequirement requirement,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ExternalAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ExternalUserClaims.Id, id.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.ExternalId, routeId);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
        }
        
        
        [Test, MoqAutoData]
        public void Then_Returns_True_If_Route_Does_Not_Have_ExternalId_But_Has_Claim(
            Guid id,
            ExternalAccountRequirement requirement,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ExternalAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ExternalUserClaims.Id, id.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var httpContext = new DefaultHttpContext(new FeatureCollection());
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
    }
}