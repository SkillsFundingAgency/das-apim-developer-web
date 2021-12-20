using System.Security.Cryptography;
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
            SetupCommandHappyPath(command);
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeTrue();
        }
        
        [Test, AutoData]
        public async Task And_No_FirstName_Then_Not_Valid(
            RegisterCommand command,
            RegisterCommandValidator validator)
        {
            //Arrange
            command.FirstName = null;
            SetupCommandHappyPath(command);
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.FirstName));
            actual.ValidationDictionary[nameof(command.FirstName)].Should()
                .Be("Enter first name");
        }
        
        [Test, AutoData]
        public async Task And_No_LastName_Then_Not_Valid(
            RegisterCommand command,
            RegisterCommandValidator validator)
        {
            //Arrange
            command.LastName = null;
            SetupCommandHappyPath(command);
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.LastName));
            actual.ValidationDictionary[nameof(command.LastName)].Should()
                .Be("Enter last name");
        }
        
        [Test, AutoData]
        public async Task And_No_EmailAddress_Then_Not_Valid(
            RegisterCommand command,
            RegisterCommandValidator validator)
        {
            //Arrange
            SetupCommandHappyPath(command);
            command.EmailAddress = null;
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.EmailAddress));
            actual.ValidationDictionary[nameof(command.EmailAddress)].Should()
                .Be("Enter an email address");
        }
        
        [Test, AutoData]
        public async Task And_MalFormed_EmailAddress_Then_Not_Valid(
            string email,
            RegisterCommand command,
            RegisterCommandValidator validator)
        {
            //Arrange
            SetupCommandHappyPath(command);
            command.EmailAddress = email;
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.EmailAddress));
            actual.ValidationDictionary[nameof(command.EmailAddress)].Should()
                .Be("Enter an email address in the correct format, like name@example.com");
        }
        
        [Test, AutoData]
        public async Task And_No_Password_Then_Not_Valid(
            RegisterCommand command,
            RegisterCommandValidator validator)
        {
            //Arrange
            SetupCommandHappyPath(command);
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
            RegisterCommand command,
            RegisterCommandValidator validator)
        {
            //Arrange
            SetupCommandHappyPath(command);
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
            RegisterCommand command,
            RegisterCommandValidator validator)
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
            RegisterCommand command,
            RegisterCommandValidator validator)
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

        private void SetupCommandHappyPath(RegisterCommand command)
        {
            command.Password = "abcABC123";
            command.ConfirmPassword = command.Password;
            command.EmailAddress = $"{command.EmailAddress}@test.com";
        }
    }
}