using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.ThirdPartyAccounts
{
    public class WhenPostingChangePassword
    {
        [Test, MoqAutoData]
        public async Task And_ValidationException_Then_Show_ChangePassword_Again(
            ChangePasswordViewModel request,
            ValidationException responseFromMediator,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<ChangePasswordCommand>(), 
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(responseFromMediator);
            
            //act
            var result = await controller.PostChangePassword(request) as ViewResult;

            //assert
            result!.ViewName.Should().Be("ChangePassword");
            result!.Model.As<ChangePasswordViewModel>().Should().BeEquivalentTo(request);
        }
        
        [Test, MoqAutoData]
        public async Task And_Mediator_Result_Success_Then_Redirect_To_Completed(
            ChangePasswordViewModel request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<ChangePasswordCommand>(command => 
                        command.Id == request.UserId
                        && command.Password == request.Password
                        && command.ConfirmPassword == request.ConfirmPassword), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            
            //act
            var result = await controller.PostChangePassword(request) as RedirectToRouteResult;

            //assert
            result!.RouteName.Should().Be(RouteNames.ThirdPartyChangePasswordComplete);
        }
    }
}