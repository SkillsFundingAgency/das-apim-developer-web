using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Subscriptions.Requests
{
    public class WhenBuildingPostCreateSubscriptionRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string accountIdentifier, string productId, string accountType)
        {
            //Act
            var actual = new PostCreateSubscriptionRequest(accountIdentifier, productId, accountType);
            
            //Assert
            actual.PostUrl.Should().Be($"subscriptions/{accountIdentifier}/products/{productId}?accountType={accountType}");
            actual.Data.Should().BeAssignableTo<object>();
        }
    }
}