using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Domain.Employers;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.AppStart;

public class WhenPopulatingAccountClaims
{
    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Stubbed_User(
        [Frozen] Mock<IEmployerAccountService> accountService,
        [Frozen] Mock<IConfiguration> configuration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        var expectedClaimEmail = "test@testing.com";
        configuration.Setup(x => x["NoAuthEmail"]).Returns(expectedClaimEmail);
        configuration.Setup(x => x["LocalStubAuth"]).Returns("true");
        
        var actual = await handler.GetClaims(null);
        
        accountService.Verify(x=>x.GetUserAccounts(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        actual.Should().ContainSingle(c => c.Value.Equals(expectedClaimEmail));
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User(
        string nameIdentifier,
        string idamsIdentifier,
        string emailAddress,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IEmployerAccountService> accountService,
        [Frozen] Mock<IConfiguration> configuration,
        [Frozen] Mock<IOptions<ApimDeveloperWeb>> apimWebConfiguration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        accountData.IsSuspended = false;
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(nameIdentifier,emailAddress)).ReturnsAsync(accountData);
        
        var actual = await handler.GetClaims(tokenValidatedContext);
        
        accountService.Verify(x=>x.GetUserAccounts(nameIdentifier,emailAddress), Times.Once);
        accountService.Verify(x=>x.GetUserAccounts(idamsIdentifier,emailAddress), Times.Never);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
        var actualClaimValue = actual.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should().Be(actualClaimValue);
        actual.FirstOrDefault(c => c.Type.Equals(ClaimTypes.AuthorizationDecision)).Should().BeNull();
    }
    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User_With_Suspended_Flag_Set(
        string nameIdentifier,
        string idamsIdentifier,
        string emailAddress,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IEmployerAccountService> accountService,
        [Frozen] Mock<IConfiguration> configuration,
        [Frozen] Mock<IOptions<ApimDeveloperWeb>> apimWebConfiguration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        accountData.IsSuspended = true;
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(nameIdentifier,emailAddress)).ReturnsAsync(accountData);
        
        var actual = await handler.GetClaims(tokenValidatedContext);
        
        accountService.Verify(x=>x.GetUserAccounts(nameIdentifier,emailAddress), Times.Once);
        accountService.Verify(x=>x.GetUserAccounts(idamsIdentifier,emailAddress), Times.Never);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
        var actualClaimValue = actual.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should().Be(actualClaimValue);
        actual.First(c => c.Type.Equals(ClaimTypes.AuthorizationDecision)).Value.Should().Be("Suspended");
    }

    private TokenValidatedContext ArrangeTokenValidatedContext(string nameIdentifier, string idamsIdentifier, string emailAddress)
    {
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
            new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, idamsIdentifier),
            new Claim(ClaimTypes.Email, emailAddress)
        });
        
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));
        return new TokenValidatedContext(new DefaultHttpContext(), new AuthenticationScheme(",","", typeof(TestAuthHandler)),
            new OpenIdConnectOptions(), Mock.Of<ClaimsPrincipal>(), new AuthenticationProperties())
        {
            Principal = claimsPrincipal
        };
    }
    
    private class TestAuthHandler : IAuthenticationHandler
    {
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }
}