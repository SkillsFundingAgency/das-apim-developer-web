using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Web.Models;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Models
{
    public class WhenBuildingSubscriptionsViewModelFromMediator
    {
        [Test, AutoData]
        public void Then_The_Model_Is_Built(GetAvailableProductsQueryResult source)
        {
            var actual = (SubscriptionsViewModel)source;

            actual.Products.Select(x=>x.DisplayName).Should().BeEquivalentTo(source.Products.Products.Select(c=>c.DisplayName));
            actual.Products.Select(x=>x.Description).Should().BeEquivalentTo(source.Products.Products.Select(c=>c.Description));
            actual.Products.Select(x=>x.Key).Should().BeEquivalentTo(source.Products.Products.Select(c=>c.Key));
            actual.Products.Select(x=>x.Name).Should().BeEquivalentTo(source.Products.Products.Select(c=>c.Name.ToLower()));
        }
    }
}