using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Models
{
    public class WhenBuildingRegisterCommandFromRegisterRequest
    {
        [Test, AutoData]
        public void Then_The_Model_Is_Built(RegisterRequest source)
        {
            var actual = (RegisterCommand)source;

            actual.Should().BeEquivalentTo(source);
        }
    }
}