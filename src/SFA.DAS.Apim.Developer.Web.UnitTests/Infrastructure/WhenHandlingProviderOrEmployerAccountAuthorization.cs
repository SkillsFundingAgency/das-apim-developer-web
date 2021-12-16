using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Infrastructure
{
    public class WhenHandlingProviderOrEmployerAccountAuthorization
    {
        [Test, MoqAutoData]
        public async Task Then_If_Provider_Calls_ProviderCheck(
            int ukprn,
            ProviderEmployerExternalAccountRequirement requirement,
            [Frozen] Mock<IProviderAccountAuthorisationHandler> providerAccountAuthorisationHandler,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderEmployerExternalAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var httpContext = new DefaultHttpContext(new FeatureCollection());
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            providerAccountAuthorisationHandler.Setup(x=>x.IsProviderAuthorised(context)).Returns(true);

            await authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
            
        }
        [Test, MoqAutoData]
        public async Task Then_If_Provider_Calls_ProviderCheck_And_Not_Valid_Does_Not_Succeed(
            int ukprn,
            ProviderEmployerExternalAccountRequirement requirement,
            [Frozen] Mock<IProviderAccountAuthorisationHandler> providerAccountAuthorisationHandler,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderEmployerExternalAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var httpContext = new DefaultHttpContext(new FeatureCollection());
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            providerAccountAuthorisationHandler.Setup(x=>x.IsProviderAuthorised(context)).Returns(false);

            await authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
            
        }
        [Test, MoqAutoData]
        public async Task Then_If_Employer_Calls_EmployerCheck(
            string accountId,
            ProviderEmployerExternalAccountRequirement requirement,
            [Frozen] Mock<IProviderAccountAuthorisationHandler> providerAccountAuthorisationHandler,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderEmployerExternalAccountAuthorizationHandler authorizationHandler)
        {
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{accountId, new EmployerIdentifier()}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var httpContext = new DefaultHttpContext(new FeatureCollection());
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            employerAccountAuthorisationHandler.Setup(x=>x.IsEmployerAuthorised(context, false)).Returns(true);

            await authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Employer_Calls_EmployerCheck_And_Not_Valid_Does_Not_Succeed(
            string accountId,
            ProviderEmployerExternalAccountRequirement requirement,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderEmployerExternalAccountAuthorizationHandler authorizationHandler)
        {
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{accountId, new EmployerIdentifier()}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var httpContext = new DefaultHttpContext(new FeatureCollection());
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            employerAccountAuthorisationHandler.Setup(x=>x.IsEmployerAuthorised(context, false)).Returns(false);

            await authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_If_External_Calls_ExternalCheck_And_Valid_Then_Succeed(
            string id,
            ProviderEmployerExternalAccountRequirement requirement,
            [Frozen] Mock<IExternalAccountAuthorizationHandler> externalAccountAuthorizationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderEmployerExternalAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ExternalUserClaims.Id, id);
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var httpContext = new DefaultHttpContext(new FeatureCollection());
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            externalAccountAuthorizationHandler.Setup(x => x.IsExternalUserAuthorised(context)).Returns(true);

            await authorizationHandler.HandleAsync(context);
            
            context.HasSucceeded.Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_External_Calls_ExternalCheck_And_Invalid_Then_Not_Succeed(
            string id,
            ProviderEmployerExternalAccountRequirement requirement,
            [Frozen] Mock<IExternalAccountAuthorizationHandler> externalAccountAuthorizationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderEmployerExternalAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(ExternalUserClaims.Id, id);
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var httpContext = new DefaultHttpContext(new FeatureCollection());
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            externalAccountAuthorizationHandler.Setup(x => x.IsExternalUserAuthorised(context)).Returns(false);

            await authorizationHandler.HandleAsync(context);
            
            context.HasSucceeded.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Matching_Claim_Then_Not_Succeed(
            string id,
            string claimName,
            ProviderEmployerExternalAccountRequirement requirement,
            [Frozen] Mock<IExternalAccountAuthorizationHandler> externalAccountAuthorizationHandler,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            [Frozen] Mock<IProviderAccountAuthorisationHandler> providerAccountAuthorisationHandler,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderEmployerExternalAccountAuthorizationHandler authorizationHandler)
        {
            var claim = new Claim(claimName, id);
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            var httpContext = new DefaultHttpContext(new FeatureCollection());
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            await authorizationHandler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
            externalAccountAuthorizationHandler.Verify(x => x.IsExternalUserAuthorised(It.IsAny<AuthorizationHandlerContext>()), Times.Never);
            providerAccountAuthorisationHandler.Verify(x => x.IsProviderAuthorised(It.IsAny<AuthorizationHandlerContext>()), Times.Never);
            employerAccountAuthorisationHandler.Verify(x => x.IsEmployerAuthorised(It.IsAny<AuthorizationHandlerContext>(), It.IsAny<bool>()), Times.Never);
        }
    }
}