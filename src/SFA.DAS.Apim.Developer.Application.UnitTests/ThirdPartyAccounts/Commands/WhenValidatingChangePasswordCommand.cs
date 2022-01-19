using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenValidatingChangePasswordCommand
    {
        [Test, AutoData]
        public async Task And_No_Password_Then_Not_Valid(
            ChangePasswordCommand command,
            ChangePasswordCommandValidator validator)
        {
            //Arrange
            command.Password = null;

            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.Password));
            actual.ValidationDictionary[nameof(command.Password)].Should()
                .Be("Enter a password");
        }
        
        [Test]
        [InlineAutoData("abcABC123", true)]
        [InlineAutoData("ABCABC123", false)]
        [InlineAutoData("abcabc123", false)]
        [InlineAutoData("abcABCabc", false)]
        [InlineAutoData("abcABC123!", true)]
        public async Task And_Password_Rules_Enforced(
            string password,
            bool isValid,
            ChangePasswordCommand command,
            ChangePasswordCommandValidator validator)
        {
            //Arrange
            command.Password = password;
            command.ConfirmPassword = password;

            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().Be(isValid);
            if (isValid) return;
            actual.ValidationDictionary.Should().ContainKey(nameof(command.Password));
            actual.ValidationDictionary[nameof(command.Password)].Should()
                .Be("Password must contain upper and lowercase letters, a number and at least 8 characters");
        }
        
        [Test, AutoData]
        public async Task And_No_ConfirmPassword_Then_Not_Valid(
            ChangePasswordCommand command,
            ChangePasswordCommandValidator validator)
        {
            //Arrange
            SetupCommandHappyPath(command);
            command.ConfirmPassword = null;

            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.ConfirmPassword));
            actual.ValidationDictionary[nameof(command.ConfirmPassword)].Should()
                .Be("Re-type password");
        }
        
        [Test, AutoData]
        public async Task And_Passwords_Dont_Match_Then_Not_Valid(
            string otherPassword,
            ChangePasswordCommand command,
            ChangePasswordCommandValidator validator)
        {
            //Arrange
            SetupCommandHappyPath(command);
            command.ConfirmPassword = otherPassword;

            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.ConfirmPassword));
            actual.ValidationDictionary[nameof(command.ConfirmPassword)].Should()
                .Be("Passwords do not match");
        }
        
        private void SetupCommandHappyPath(ChangePasswordCommand command)
        {
            command.Password = "abcABC123";
            command.ConfirmPassword = command.Password;
        }
    }
}