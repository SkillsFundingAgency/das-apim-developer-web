using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Subscriptions.Requests
{
    public class WhenBuildingGetProductSubscriptionRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string accountIdentifier, string productId, string accountType)
        {
            //Act
            var actual = new GetProductSubscriptionRequest(accountIdentifier, productId, accountType);
            
            //Assert
            actual.GetUrl.Should().Be($"subscriptions/{accountIdentifier}/products/{productId}?accountType={accountType}");
        }
    }
}