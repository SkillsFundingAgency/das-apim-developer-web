using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.ThirdPartyAccounts
{
    public class WhenPostingRegister
    {
        [Test, MoqAutoData]
        public async Task Then_Redirect_To_ThirdPartyConfirmEmail(
            RegisterRequest request,
            RegisterCommandResult responseFromMediator,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RegisterCommand>(command => 
                        command.Id != Guid.Empty
                        && command.FirstName == request.FirstName 
                        && command.LastName == request.LastName
                        && command.EmailAddress == request.EmailAddress
                        && command.Password == request.Password
                        && command.ConfirmPassword == request.ConfirmPassword), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(responseFromMediator);
            
            var result = await controller.PostRegister(request) as RedirectToRouteResult;

            result!.RouteName.Should().Be(RouteNames.ThirdPartyConfirmEmail);
            result.RouteValues["id"].Should().NotBeNull();
            result.RouteValues["id"].Should().Be(responseFromMediator.Id);
        }
        
        [Test, MoqAutoData]
        public async Task And_ValidationException_Then_Show_Register_Again(
            RegisterRequest request,
            ValidationException responseFromMediator,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<RegisterCommand>(), 
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(responseFromMediator);
            
            var result = await controller.PostRegister(request) as ViewResult;

            result!.ViewName.Should().Be("Register");
            var model = result.Model as RegisterViewModel;
            model!.Should().BeEquivalentTo(request);
        }
    }
}