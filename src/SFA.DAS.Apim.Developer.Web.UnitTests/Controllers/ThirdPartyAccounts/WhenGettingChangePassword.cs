using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.ThirdPartyAccounts
{
    public class WhenGettingChangePassword
    {
        [Test, MoqAutoData]
        public void And_Id_Not_Decode_Then_Redirect_To_Signup(
            Guid decodedUserId,
            string encodedUserId,
            [Frozen] Mock<IDataProtectorService> mockDataProtector,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            mockDataProtector
                .Setup(service => service.DecodeData(encodedUserId))
                .Returns((Guid?)null);
            
            //act
            var result = controller.ChangePassword(encodedUserId) as RedirectToRouteResult;

            //assert
            result!.RouteName.Should().Be(RouteNames.ThirdPartyRegister);
        }
        
        [Test, MoqAutoData]
        public void And_Complete_Successful_Then_Show_View(
            Guid decodedUserId,
            string encodedUserId,
            [Frozen] Mock<IDataProtectorService> mockDataProtector,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Greedy] ThirdPartyAccountsController controller)
        {
            //arrange
            mockDataProtector
                .Setup(service => service.DecodeData(encodedUserId))
                .Returns(decodedUserId);
            
            //act
            var result = controller.ChangePassword(encodedUserId) as ViewResult;

            //assert
            result!.Model.As<ChangePasswordViewModel>().UserId.Should().Be(decodedUserId);
        }
    }
}