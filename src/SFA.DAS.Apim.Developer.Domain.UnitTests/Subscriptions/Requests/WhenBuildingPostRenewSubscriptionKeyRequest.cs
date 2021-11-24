using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Subscriptions.Requests
{
    public class WhenBuildingPostRenewSubscriptionKeyRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string accountIdentifier, string productId)
        {
            var actual = new PostRenewSubscriptionKeyRequest(accountIdentifier, productId);

            actual.PostUrl.Should().Be($"subscriptions/{accountIdentifier}/renew/{productId}");
        }
    }
}