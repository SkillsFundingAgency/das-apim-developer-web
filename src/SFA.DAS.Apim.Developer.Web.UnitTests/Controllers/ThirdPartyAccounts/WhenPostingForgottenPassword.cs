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
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.SendChangePasswordEmail;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Queries.GetUser;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.ThirdPartyAccounts
{
    public class WhenPostingForgottenPassword
    {
        [Test, MoqAutoData]
        public async Task And_ValidationException_Then_Show_ForgottenPassword_Again(
            ForgottenPasswordViewModel request,
            ValidationException responseFromMediator,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Frozen] Mock<IMediator> mockMediator,
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
                    It.IsAny<GetUserQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(responseFromMediator);

            //act
            var result = await controller.PostForgottenPassword(request) as ViewResult;

            //assert
            result!.ViewName.Should().Be("ForgottenPassword");
            var model = result.Model as ForgottenPasswordViewModel;
            model!.Should().BeEquivalentTo(request);
        }
        
        [Test, MoqAutoData]
        public async Task And_User_Not_Found_Then_Show_Complete(
            ForgottenPasswordViewModel request,
            ValidationException responseFromMediator,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Frozen] Mock<IMediator> mockMediator,
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
                    It.IsAny<GetUserQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetUserResponse());

            //act
            var result = await controller.PostForgottenPassword(request) as RedirectToRouteResult;

            //assert
            result!.RouteName.Should().Be(RouteNames.ThirdPartyForgottenPasswordComplete);
            mockMediator.Verify(mediator => mediator.Send(It.IsAny<SendChangePasswordEmailCommand>(), CancellationToken.None), 
                Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Redirect_To_ForgottenPasswordComplete(
            ForgottenPasswordViewModel request,
            string changePasswordUrl,
            string encodedUserId,
            Guid idFromMediator,
            GetUserResponse mediatorResponse,
            [Frozen] Mock<IDataProtectorService> mockDataProtector,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            mediatorResponse.User.Id = idFromMediator.ToString();
            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetUserQuery>(query =>
                        query.EmailAddress == request.EmailAddress),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);
            UrlRouteContext routeValues = null;
            mockUrlHelper
                .Setup(helper => helper.RouteUrl(It.Is<UrlRouteContext>(context =>
                    context.RouteName == RouteNames.ThirdPartyChangePassword)))
                .Returns(changePasswordUrl)
                .Callback<UrlRouteContext>(context => routeValues = context);
            mockDataProtector
                .Setup(service => service.EncodedData(Guid.Parse(mediatorResponse.User.Id)))
                .Returns(encodedUserId);
            controller.Url = mockUrlHelper.Object;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<SendChangePasswordEmailCommand>(command =>
                        command.Id.ToString() == mediatorResponse.User.Id
                        && command.FirstName == mediatorResponse.User.FirstName
                        && command.LastName == mediatorResponse.User.LastName
                        && command.Email == request.EmailAddress
                        && command.ChangePasswordUrl == changePasswordUrl),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            //act
            var result = await controller.PostForgottenPassword(request) as RedirectToRouteResult;

            //assert
            result!.RouteName.Should().Be(RouteNames.ThirdPartyForgottenPasswordComplete);
            mockMediator.Verify(mediator => mediator.Send(It.IsAny<SendChangePasswordEmailCommand>(), CancellationToken.None), 
                Times.Once);
        }
    }
}