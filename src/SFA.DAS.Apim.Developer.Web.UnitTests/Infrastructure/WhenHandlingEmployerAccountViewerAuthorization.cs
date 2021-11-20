using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Infrastructure
{
    public class WhenHandlingEmployerAccountViewerAuthorization
    {
        [Test, MoqAutoData]
        public async Task Then_Calls_Authorization_Service_And_Checks_AuthHandler(
            EmployerViewerRoleRequirement requirement,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            EmployerViewerAuthorizationHandler handler)
        {
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity()});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            employerAccountAuthorisationHandler.Setup(x=>x.IsEmployerAuthorised(context, true)).Returns(true);
            
            //Act
            await handler.HandleAsync(context);
            
            //Assert
            context.HasSucceeded.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_If_Not_Valid_Then_Does_Not_Succeed_Requirement(
            EmployerViewerRoleRequirement requirement,
            [Frozen] Mock<IEmployerAccountAuthorisationHandler> employerAccountAuthorisationHandler,
            EmployerViewerAuthorizationHandler handler)
        {
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity()});
            var context = new AuthorizationHandlerContext(new [] {requirement}, claimsPrinciple, null);
            employerAccountAuthorisationHandler.Setup(x=>x.IsEmployerAuthorised(context, true)).Returns(false);
            
            //Act
            await handler.HandleAsync(context);
            
            //Assert
            context.HasSucceeded.Should().BeFalse();
        }
    }
}