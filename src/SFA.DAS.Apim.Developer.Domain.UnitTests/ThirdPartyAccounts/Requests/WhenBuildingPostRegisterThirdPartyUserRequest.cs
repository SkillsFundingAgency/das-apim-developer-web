using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.ThirdPartyAccounts.Requests
{
    public class WhenBuildingPostRegisterThirdPartyUserRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            Mock<IRegisterThirdPartyAccountData> mockData)
        {
            //Act
            var data = new PostRegisterThirdPartyAccountData(mockData.Object);
            var actual = new PostRegisterThirdPartyAccountRequest(data);
            
            //Assert
            actual.PostUrl.Should().Be($"users/{mockData.Object.Id}");
            actual.Data.Should().Be(data);
        }
    }
}