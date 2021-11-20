using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Extensions;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Extensions
{
    public class WhenGettingEnumDescription
    {
        [Test]
        public void Then_Returns_Description_From_Attr()
        {
            EnumForTesting.ForTesting.GetDescription().Should().Be("Something for testing");
        }

        [Test]
        public void And_No_Attr_Then_Returns_Empty()
        {
            EnumForTesting.NoDescription.GetDescription().Should().BeEmpty();
        }

        public enum EnumForTesting
        {
            [System.ComponentModel.Description("Something for testing")]
            ForTesting,
            NoDescription
        }
    }
}