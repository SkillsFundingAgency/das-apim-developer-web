using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Products.Queries.GetProduct;
using SFA.DAS.Apim.Developer.Web.Models;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Models
{
    public class WhenBuildingApiProductViewModelFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetProductQueryResult source)
        {
            var actual = (ApiProductViewModel)source;
            
            actual.Should().BeEquivalentTo(source.Product);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            var source = new GetProductQueryResult
            {
                Product = null
            };

            var actual = (ApiProductViewModel)source;

            actual.Should().BeNull();
        }
    }
}