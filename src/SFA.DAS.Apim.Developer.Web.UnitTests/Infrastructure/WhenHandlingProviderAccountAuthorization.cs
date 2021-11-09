using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Infrastructure
{
    public class WhenHandlingProviderAccountAuthorization
    {
        [Test, MoqAutoData]
        public void Then_Returns_True_If_Provider_Is_Authorized(
            int ukprn,
            ProviderAccountRequirement requirement,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.Ukprn, ukprn);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public void Then_Returns_False_If_Claim_Does_Not_Match_Route_Value(
            int ukprn,
            int ukprnRoute,
            ProviderAccountRequirement requirement,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.Ukprn, ukprnRoute);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
        }
        
        [Test, MoqAutoData]
        public void Then_Returns_False_If_Claim_Does_Not_Exist(
            int ukprnRoute,
            ProviderAccountRequirement requirement,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderAccountAuthorizationHandler authorizationHandler)
        {
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity()});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.Ukprn, ukprnRoute);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
        }
        
        [Test, MoqAutoData]
        public void Then_Returns_False_If_Route_Does_Not_Have_Ukprn(
            int ukprn,
            ProviderAccountRequirement requirement,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            
            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
        }
    }
}