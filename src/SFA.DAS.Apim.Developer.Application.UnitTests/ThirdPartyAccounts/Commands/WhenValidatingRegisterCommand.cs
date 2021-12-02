using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenValidatingRegisterCommand
    {
        [Test, AutoData]
        public async Task And_All_Fields_Present_Then_Valid(
            RegisterCommand command,
            RegisterCommandValidator validator)
        {
            //Arrange
            command.EmailAddress = $"{command.EmailAddress}@test.com";
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeTrue();
        }
    }
}