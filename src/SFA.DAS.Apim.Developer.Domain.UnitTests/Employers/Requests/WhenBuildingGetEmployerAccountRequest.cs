using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Employers.Requests
{
    public class WhenBuildingGetEmployerAccountRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built_And_Url_Encoded(string userId)
        {
            var actual = new GetUserAccountsRequest(userId);

            actual.GetUrl.Should().Be($"accountusers/{userId}/accounts");
        }
    }
}