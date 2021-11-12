using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Subscriptions;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Subscriptions
{
    public class WhenConvertingFromApiResponseToProductSubscriptions
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped(GetAvailableProductSubscriptionsResponse source)
        {
            var actual = (ProductSubscriptions) source.Products;

            actual.Products.Should().BeEquivalentTo(source.Products);
        }

        [Test, AutoData]
        public void Then_If_Null_Then_Empty_Returned(GetAvailableProductSubscriptionsResponse source)
        {
            source.Products = null;
            var actual = (ProductSubscriptions) source.Products;

            actual.Products.Should().BeEmpty();
        }
    }
}