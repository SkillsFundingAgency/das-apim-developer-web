using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
            string confirmUrl,
            string encodedUserId,
            RegisterCommandResult responseFromMediator,
            [Frozen] Mock<IDataProtectorService> mockDataProtector,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            UrlRouteContext confirmEmailRouteValues = null;
            mockUrlHelper
                .Setup(helper => helper.RouteUrl(It.Is<UrlRouteContext>(context =>
                    context.RouteName == RouteNames.ThirdPartyRegisterComplete)))
                .Returns(confirmUrl)
                .Callback<UrlRouteContext>(context => confirmEmailRouteValues = context);
            mockDataProtector
                .Setup(service => service.EncodedData(It.IsAny<Guid>()))
                .Returns(encodedUserId);
            controller.Url = mockUrlHelper.Object;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RegisterCommand>(command => 
                        command.Id != Guid.Empty
                        && command.FirstName == request.FirstName 
                        && command.LastName == request.LastName
                        && command.EmailAddress == request.EmailAddress
                        && command.Password == request.Password
                        && command.ConfirmPassword == request.ConfirmPassword
                        && command.ConfirmUrl == confirmUrl), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(responseFromMediator);
            
            //act
            var result = await controller.PostRegister(request) as RedirectToRouteResult;

            //assert
            result!.RouteName.Should().Be(RouteNames.ThirdPartyAwaitingConfirmEmail);
            confirmEmailRouteValues.Values.Should().BeEquivalentTo(new {id = encodedUserId});
        }
        
        [Test, MoqAutoData]
        public async Task And_ValidationException_Then_Show_Register_Again(
            RegisterRequest request,
            ValidationException responseFromMediator,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            controller.Url = mockUrlHelper.Object;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<RegisterCommand>(), 
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(responseFromMediator);
            
            //act
            var result = await controller.PostRegister(request) as ViewResult;

            //assert
            result!.ViewName.Should().Be("Register");
            var model = result.Model as RegisterViewModel;
            model!.Should().BeEquivalentTo(request);
        }
    }
}