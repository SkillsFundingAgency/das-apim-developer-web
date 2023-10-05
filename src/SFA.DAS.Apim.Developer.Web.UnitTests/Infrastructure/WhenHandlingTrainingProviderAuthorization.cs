using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Providers.Api.Responses;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Infrastructure
{
    public class WhenHandlingTrainingProviderAuthorization
    {
        [Test, MoqAutoData]
        public async Task Then_The_ProviderStatus_Is_Valid_And_True_Returned(
            long ukprn,
            ProviderAccountResponse apiResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            TrainingProviderAllRolesRequirement requirement,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            apiResponse.CanAccessService = true;
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
            var context = new AuthorizationHandlerContext(new[] { requirement }, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.Ukprn, ukprn);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            
            trainingProviderService.Setup(x => x.GetProviderStatus(ukprn)).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_ProviderDetails_Is_InValid_And_False_Returned(
            long ukprn,
            ProviderAccountResponse apiResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            TrainingProviderAllRolesRequirement requirement,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            apiResponse.CanAccessService = false;
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
            var context = new AuthorizationHandlerContext(new[] { requirement }, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.Ukprn, ukprn);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            trainingProviderService.Setup(x => x.GetProviderStatus(ukprn)).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_The_ProviderDetails_Is_Null_And_False_Returned(
            long ukprn,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            TrainingProviderAllRolesRequirement requirement,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
            var context = new AuthorizationHandlerContext(new[] { requirement }, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add(RouteValues.Ukprn, ukprn);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            trainingProviderService.Setup(x => x.GetProviderStatus(ukprn)).ReturnsAsync((ProviderAccountResponse)null!);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeFalse();
        }
    }
}
