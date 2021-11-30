using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Subscriptions.Requests
{
    public class WhenBuildingGetAvailableProductSubscriptionsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string accountType, string accountIdentifier)
        {
            var actual = new GetAvailableProductSubscriptionsRequest(accountType, accountIdentifier);

            actual.GetUrl.Should().Be($"subscriptions/{accountIdentifier}/products?accountType={accountType}");
        }
    }
}