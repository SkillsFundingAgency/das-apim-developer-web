using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Products;
using SFA.DAS.Apim.Developer.Domain.Products.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Products
{
    public class WhenConvertingFromApiResponseToProduct
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetProductResponse source)
        {
            var actual = (Product)source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}