using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Models
{
    public class WhenBuildingRegisterViewModelFromRegisterRequest
    {
        [Test, AutoData]
        public void Then_The_Model_Is_Built(RegisterRequest source)
        {
            var actual = (RegisterViewModel)source;

            actual.Should().BeEquivalentTo(source);
        }
    }
}