using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.ThirdPartyAccounts.Requests
{
    public class WhenBuildingAPostSendChangePasswordEmailRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            Mock<ISendChangePasswordEmailData> mockData)
        {
            //Act
            var data = new PostSendChangePasswordEmailRequestData(mockData.Object);
            var actual = new PostSendChangePasswordEmailRequest(data);
            
            //Assert
            actual.PostUrl.Should().Be($"users/{mockData.Object.Id}/send-change-password-email");
            actual.Data.Should().BeEquivalentTo(mockData.Object);
        }
    }
}