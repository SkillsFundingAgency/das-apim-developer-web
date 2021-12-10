using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser;
using SFA.DAS.Apim.Developer.Domain.Validation;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.Apim.Developer.Domain.Validation.ValidationResult;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.ThirdPartyAccounts
{
    public class WhenPostingSignin
    {
        [Test, MoqAutoData]
        public async Task Then_If_Model_Is_Not_Valid_Then_Signin_View_Returned(
            LoginViewModel viewModel,
            [Greedy] ThirdPartyAccountsController controller)
        {
            controller.ModelState.AddModelError("key", "error message");
            
            var actual = await controller.PostLogin(viewModel) as ViewResult;
            
            actual.ViewName.Should().Be("Login");
            actual.Model.Should().BeAssignableTo<LoginViewModel>();
        }

        [Test, MoqAutoData]
        public async Task Then_If_Model_Is_Valid_Then_Command_Handled(
            LoginViewModel viewModel,
            AuthenticateUserCommandResponse response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            response.UserDetails.Authenticated = true;
            mediator.Setup(x =>
                x.Send(
                    It.Is<AuthenticateUserCommand>(c =>
                        c.EmailAddress.Equals(viewModel.EmailAddress) && c.Password.Equals(viewModel.Password)),
                    CancellationToken.None)).ReturnsAsync(response);
            
            var actual = await controller.PostLogin(viewModel) as RedirectToRouteResult;

            actual.RouteName.Should().Be(RouteNames.ExternalApiHub);
            actual.RouteValues["externalId"].Should().Be(response.UserDetails.Id);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Model_Is_Valid_And_Command_Handler_Returns_Not_Authenticated_Error_Shown(
            LoginViewModel viewModel,
            AuthenticateUserCommandResponse response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            response.UserDetails.Authenticated = false;
            mediator.Setup(x =>
                x.Send(
                    It.Is<AuthenticateUserCommand>(c =>
                        c.EmailAddress.Equals(viewModel.EmailAddress) && c.Password.Equals(viewModel.Password)),
                    CancellationToken.None)).ReturnsAsync(response);
            
            var actual = await controller.PostLogin(viewModel)  as ViewResult;
            
            actual.ViewName.Should().Be("Login");
            actual.Model.Should().BeAssignableTo<LoginViewModel>();
            controller.ModelState.IsValid.Should().BeFalse();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Model_Is_Valid_And_Command_Handler_Returns_Null_Error_Shown(
            LoginViewModel viewModel,
            AuthenticateUserCommandResponse response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            response.UserDetails = null;
            mediator.Setup(x =>
                x.Send(
                    It.Is<AuthenticateUserCommand>(c =>
                        c.EmailAddress.Equals(viewModel.EmailAddress) && c.Password.Equals(viewModel.Password)),
                    CancellationToken.None)).ReturnsAsync(response);
            
            var actual = await controller.PostLogin(viewModel)  as ViewResult;
            
            actual.ViewName.Should().Be("Login");
            actual.Model.Should().BeAssignableTo<LoginViewModel>();
            controller.ModelState.IsValid.Should().BeFalse();
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_If_Model_Is_Valid_And_Command_Handler_Returns_Validation_Error_Login_View_Shown(
            LoginViewModel viewModel,
            AuthenticateUserCommandResponse response,
            string errorMessage,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            var validationResult = new ValidationResult
            {
                ValidationDictionary = { { "email", errorMessage } }
            };
            response.UserDetails.Authenticated = false;
            mediator.Setup(x =>
                x.Send(
                    It.Is<AuthenticateUserCommand>(c =>
                        c.EmailAddress.Equals(viewModel.EmailAddress) && c.Password.Equals(viewModel.Password)),
                    CancellationToken.None)).ThrowsAsync(new ValidationException(validationResult.DataAnnotationResult,null, null));
            
            var actual = await controller.PostLogin(viewModel)  as ViewResult;
            
            actual.ViewName.Should().Be("Login");
            actual.Model.Should().BeAssignableTo<LoginViewModel>();
            controller.ModelState["email"].Errors.First().ErrorMessage.Should().Be(errorMessage);
        }
    }
}