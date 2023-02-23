using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Subscriptions.Requests
{
    public class WhenBuildingDeleteSubscriptionRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string accountIdentifier, string productId)
        {
            //Act
            var actual = new DeleteSubscriptionKeyRequest(accountIdentifier, productId);

            //Assert
            actual.DeleteUrl.Should().Be($"subscriptions/{accountIdentifier}/delete/{productId}");
        }
    }
}