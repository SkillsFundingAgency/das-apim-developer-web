using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.ThirdPartyAccounts.Requests
{
    public class WhenBuildingGetUserRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Set(string email)
        {
            email = $"{email}!@£$%£$%+15";
            
            var actual = new GetUserRequest(email);

            actual.GetUrl.Should().Be($"users?email={HttpUtility.UrlEncode(email)}");
        }
    }
}