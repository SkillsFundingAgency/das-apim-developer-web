using System.Collections.Generic;
using System.Security.Claims;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Employers.Api;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Infrastructure
{
    public class WhenHandlingEmployerAccountAuthorization
    {
        [Test, MoqAutoData]
        public void Then_Returns_True_If_Employer_Is_Authorized(
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            employerIdentifier.Role = "Owner";
            employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});

            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
                
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, employerIdentifier.AccountId);
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, false);

            //Assert
            Assert.IsTrue(result);
        }
        
        [Test, MoqAutoData]
        public void Then_Returns_False_If_Employer_Is_Not_Authorized(
            string accountId,
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            employerIdentifier.Role = "Owner";
            employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});

            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
                
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, accountId.ToUpper());
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, false);

            //Assert
            Assert.IsFalse(result);
        }

        [Test, MoqAutoData]
        public void Then_If_Not_In_Context_Claims_EmployerAccountService_Checked_And_True_Returned_If_Exists(
            string accountId,
            string userId,
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            GetEmployerUserAccountItem serviceResponse,
            [Frozen] Mock<HttpContext> httpContext,
            [Frozen] Mock<IEmployerAccountService> employerAccountService,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            serviceResponse.AccountId = accountId.ToUpper();
            serviceResponse.Role = "Owner";
            employerAccountService.Setup(x => x.GetUserAccounts(userId))
                .ReturnsAsync(new GetEmployerUserAccounts
                {
                    EmployerAccounts = new List<GetEmployerUserAccountItem>{ serviceResponse }
                });
            
            var userClaim = new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, userId);
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var employerAccountClaim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {employerAccountClaim, userClaim})});
            var contextFilter = new AuthorizationFilterContext(new ActionContext(httpContext.Object,new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, accountId.ToUpper());
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, false);
            
            //Assert
            Assert.IsTrue(result);
            
        }
        
        [Test, MoqAutoData]
        public void Then_If_Not_In_Context_Claims_EmployerAccountService_Checked_And_False_Returned_If_Not_Exists(
            string accountId,
            string userId,
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            GetEmployerUserAccountItem serviceResponse,
            [Frozen] Mock<IEmployerAccountService> employerAccountService,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            serviceResponse.AccountId = serviceResponse.AccountId.ToUpper();
            serviceResponse.Role = "Owner";
            employerAccountService.Setup(x => x.GetUserAccounts(userId))
                .ReturnsAsync(new GetEmployerUserAccounts
                {
                    EmployerAccounts = new List<GetEmployerUserAccountItem>{ serviceResponse }
                });
            
            var userClaim = new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, userId);
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var employerAccountClaim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {employerAccountClaim, userClaim})});
            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, accountId.ToUpper());
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, false);
            
            //Assert
            Assert.IsFalse(result);
        }
        
        [Test, MoqAutoData]
        public void Then_Returns_False_If_Employer_Is_Authorized_But_Viewer_Role_Not_Allowed(
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            employerIdentifier.Role = "Viewer";
            employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});

            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
                
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, employerIdentifier.AccountId);
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, false);

            //Assert
            Assert.IsFalse(result);
        }
        
        [Test, MoqAutoData]
        public void Then_Returns_True_If_Employer_Is_Authorized_But_Has_Viewer_Role(
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            employerIdentifier.Role = "Viewer";
            employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});

            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
                
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, employerIdentifier.AccountId);
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, true);

            //Assert
            Assert.IsTrue(result);
        }
        
        [Test, MoqAutoData]
        public void Then_Returns_False_If_Employer_Is_Authorized_But_Has_Invalid_Role_But_Should_Allow_All_known_Roles(
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            employerIdentifier.Role = "Viewer-Owner-Transactor";
            employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});

            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
                
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, employerIdentifier.AccountId);
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, true);

            //Assert
            Assert.IsFalse(result);
        }
        
        
        [Test, MoqAutoData]
        public void Then_Returns_False_If_AccountId_Not_In_Url(
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            employerIdentifier.Role = "Viewer";
            employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});

            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
                
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, true);

            //Assert
            Assert.IsFalse(result);
        }

        [Test, MoqAutoData]
        public void Then_Returns_False_If_No_Matching_AccountIdentifier_Claim_Found(
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            employerIdentifier.Role = "Viewer-Owner-Transactor";
            employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
            var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
            var claim = new Claim("SomeOtherClaim", JsonConvert.SerializeObject(employerAccounts));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});

            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
                
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, employerIdentifier.AccountId);
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, true);

            //Assert
            Assert.IsFalse(result);
        }

        [Test, MoqAutoData]
        public void Then_Returns_False_If_The_Claim_Cannot_Be_Deserialized(
            EmployerIdentifier employerIdentifier,
            EmployerAccountRequirement requirement,
            EmployerAccountAuthorizationHandler authorizationHandler)
        {
            //Arrange
            employerIdentifier.Role = "Owner";
            employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
            var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerIdentifier));
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});

            var contextFilter = new AuthorizationFilterContext(new ActionContext(Mock.Of<HttpContext>(),new RouteData(), new ActionDescriptor()),new List<IFilterMetadata> {});
                
            var context = new AuthorizationHandlerContext(new[] {requirement}, claimsPrinciple, contextFilter);
            var filter = context.Resource as AuthorizationFilterContext;
            filter.HttpContext.Items = new Dictionary<object, object>();
            filter.RouteData.Values.Add(RouteValues.EmployerAccountId, employerIdentifier.AccountId);
            
            //Act
            var result = authorizationHandler.IsEmployerAuthorised(context, false);

            //Assert
            Assert.IsFalse(result);
        }
    }
}