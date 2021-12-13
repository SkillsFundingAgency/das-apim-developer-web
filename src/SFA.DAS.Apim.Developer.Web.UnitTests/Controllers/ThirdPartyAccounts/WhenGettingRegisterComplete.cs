using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.VerifyRegistration;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.ThirdPartyAccounts
{
    public class WhenGettingRegisterComplete
    {
        [Test, MoqAutoData]
        public async Task And_Id_Not_Decode_Then_Redirect_To_Signup(
            Guid decodedUserId,
            string encodedUserId,
            [Frozen] Mock<IDataProtectorService> mockDataProtector,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            mockDataProtector
                .Setup(service => service.DecodeData(encodedUserId))
                .Returns((Guid?)null);
            
            //act
            var result = await controller.RegisterComplete(encodedUserId) as RedirectToRouteResult;

            //assert
            result!.RouteName.Should().Be(RouteNames.ThirdPartyRegister);
        }
        
        [Test, MoqAutoData]
        public async Task And_Error_From_Handler_Then_Redirect_To_Signup(
            Guid decodedUserId,
            string encodedUserId,
            InvalidOperationException exception,
            [Frozen] Mock<IDataProtectorService> mockDataProtector,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            mockDataProtector
                .Setup(service => service.DecodeData(encodedUserId))
                .Returns(decodedUserId);
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<VerifyRegistrationCommand>(command => 
                        command.Id == decodedUserId), 
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);
            
            //act
            var result = await controller.RegisterComplete(encodedUserId) as RedirectToRouteResult;

            //assert
            result!.RouteName.Should().Be(RouteNames.ThirdPartyRegister);
        }
        
        [Test, MoqAutoData]
        public async Task And_Complete_Successful_Then_Show_View(
            Guid decodedUserId,
            string encodedUserId,
            [Frozen] Mock<IDataProtectorService> mockDataProtector,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            mockDataProtector
                .Setup(service => service.DecodeData(encodedUserId))
                .Returns(decodedUserId);
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<VerifyRegistrationCommand>(command => 
                        command.Id == decodedUserId), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            
            //act
            var result = await controller.RegisterComplete(encodedUserId) as ViewResult;

            //assert
            result!.Model.Should().Be(decodedUserId);
        }
    }
}