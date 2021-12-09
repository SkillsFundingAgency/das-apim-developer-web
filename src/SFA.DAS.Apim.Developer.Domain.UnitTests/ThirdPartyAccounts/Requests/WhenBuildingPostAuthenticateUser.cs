using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.ThirdPartyAccounts.Requests
{
    public class WhenBuildingPostAuthenticateUser
    {
        [Test, AutoData]
        public void Then_The_Url_And_Data_Is_Correctly_Set(string email, string password)
        {
            var actual = new PostAuthenticateUserRequest(email, password);

            actual.PostUrl.Should().Be("users/authenticate");
            ((PostAuthenticateUserRequestData)actual.Data).Email.Should().Be(email);
            ((PostAuthenticateUserRequestData)actual.Data).Password.Should().Be(password);
        }
    }
}