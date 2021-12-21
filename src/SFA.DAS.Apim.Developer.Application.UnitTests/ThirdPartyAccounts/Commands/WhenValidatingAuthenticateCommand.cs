using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenValidatingAuthenticateCommand
    {
        [Test]
        public async Task Then_If_No_Email_Address_Not_Valid()
        {
            var validator = new AuthenticateUserCommandValidator();

            var actual = await validator.ValidateAsync(new AuthenticateUserCommand
            {
                EmailAddress = "test"
            });

            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary[nameof(AuthenticateUserCommand.EmailAddress)].Should().Be("Enter an email address in the correct format, like name@example.com");
        }

        [Test]
        public async Task Then_If_Email_Address_Then_Valid()
        {
            var validator = new AuthenticateUserCommandValidator();

            var actual = await validator.ValidateAsync(new AuthenticateUserCommand
            {
                EmailAddress = "test@test.com"
            });

            actual.IsValid().Should().BeTrue();
        }
    }
}