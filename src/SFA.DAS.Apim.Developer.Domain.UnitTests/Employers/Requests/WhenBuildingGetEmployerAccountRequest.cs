using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Employers.Requests
{
    public class WhenBuildingGetEmployerAccountRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string userId)
        {
            var actual = new GetUserAccountsRequest(userId);

            actual.GetUrl.Should().Be($"accounts/{userId}");
        }
    }
}