using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Products.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Products.Requests
{
    public class WhenBuildingGetProductRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string productId)
        {
            var actual = new GetProductRequest(productId);

            actual.GetUrl.Should().Be($"products/{productId}");
        }
    }
}