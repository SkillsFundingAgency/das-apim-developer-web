using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Infrastructure;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Subscriptions
{
    public class WhenGettingApiList
    {
        [Test, MoqAutoData]
        public void Then_If_Provider_Redirects_With_Ukprn(
            int ukprn,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            //Arrange
            serviceParameters.Object.AuthenticationType = AuthenticationType.Provider;
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var controller = new SubscriptionsController(Mock.Of<IMediator>(), serviceParameters.Object);
            controller.ControllerContext = new ControllerContext() {HttpContext = new DefaultHttpContext() { User = claimsPrinciple }};
            
            //Act
            var actual = controller.ApiList() as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ProviderApiHub);
            actual.RouteValues["ukprn"].Should().Be(ukprn.ToString());
        }    
        [Test, MoqAutoData]
        public void Then_If_Employer_Redirects_With_EmployerAccountId(
            EmployerIdentifier employerIdentifier,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            //Arrange
            serviceParameters.Object.AuthenticationType = AuthenticationType.Employer;
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var controller = new SubscriptionsController(Mock.Of<IMediator>(), serviceParameters.Object);
            controller.ControllerContext = new ControllerContext() {HttpContext = new DefaultHttpContext() { User = claimsPrinciple }};
            
            //Act
            var actual = controller.ApiList() as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.EmployerApiHub);
            actual.RouteValues["employerAccountId"].Should().Be(employerAccounts.FirstOrDefault().Key);
        }
        [Test, MoqAutoData]
        public void Then_If_External_Redirects_With_ExternalId(
            Guid id,
            [Frozen] Mock<ServiceParameters> serviceParameters)
        {
            //Arrange
            serviceParameters.Object.AuthenticationType = AuthenticationType.External;
            var claim = new Claim(ExternalUserClaims.Id, id.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var controller = new SubscriptionsController(Mock.Of<IMediator>(), serviceParameters.Object);
            controller.ControllerContext = new ControllerContext() {HttpContext = new DefaultHttpContext() { User = claimsPrinciple }};
            
            //Act
            var actual = controller.ApiList() as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ExternalApiHub);
            actual.RouteValues["externalId"].Should().Be(id.ToString());
        }
    }
}