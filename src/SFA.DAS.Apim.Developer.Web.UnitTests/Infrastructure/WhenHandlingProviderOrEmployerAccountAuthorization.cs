using System.Collections.Generic;
using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Infrastructure
{
    public class WhenHandlingProviderOrEmployerAccountAuthorization
    {
        [Test, MoqAutoData]
        public void Then_If_Provider_Calls_ProviderCheck(
            int ukprn,
            ProviderOrEmployerAccountRequirement requirement,
            [Frozen] Mock<IProviderAccountAuthorisationHandler> providerAccountAuthorisationHandler,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderOrEmployerAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.Ukprn, ukprn);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            providerAccountAuthorisationHandler.Setup(x=>x.IsProviderAuthorised(context)).Returns(true);

            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
            
        }
        [Test, MoqAutoData]
        public void Then_If_Provider_Calls_ProviderCheck_And_Not_Valid_Does_Not_Succeed(
            int ukprn,
            ProviderOrEmployerAccountRequirement requirement,
            [Frozen] Mock<IProviderAccountAuthorisationHandler> providerAccountAuthorisationHandler,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderOrEmployerAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.Ukprn, ukprn);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            providerAccountAuthorisationHandler.Setup(x=>x.IsProviderAuthorised(context)).Returns(false);

            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
            
        }
        [Test, MoqAutoData]
        public void Then_If_Employer_Calls_EmployerCheck(
            string accountId,
            ProviderOrEmployerAccountRequirement requirement,
            [Frozen] Mock<IProviderAccountAuthorisationHandler> providerAccountAuthorisationHandler,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderOrEmployerAccountAuthorizationHandler authorizationHandler)
        {
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{accountId, new EmployerIdentifier()}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.EmployerAccountId, accountId);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            employerAccountAuthorisationHandler.Setup(x=>x.IsEmployerAuthorised(context, false)).Returns(true);

            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public void Then_If_Employer_Calls_EmployerCheck_And_Not_Valid_Does_Not_Succeed(
            string accountId,
            ProviderOrEmployerAccountRequirement requirement,
            [Frozen] Mock<IProviderAccountAuthorisationHandler> providerAccountAuthorisationHandler,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderOrEmployerAccountAuthorizationHandler authorizationHandler)
        {
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{accountId, new EmployerIdentifier()}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.EmployerAccountId, accountId);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            employerAccountAuthorisationHandler.Setup(x=>x.IsEmployerAuthorised(context, false)).Returns(false);

            authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
        }
    }
}