using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenHandlingAuthenticateCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Response_Returned(
            AuthenticateUserCommand command,
            UserDetails userDetails,
            [Frozen] Mock<IUserService> userService,
            AuthenticateUserCommandHandler handler)
        {
            //Arrange
            userService.Setup(x => x.AuthenticateUser(command.Email, command.Password)).ReturnsAsync(userDetails);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.UserDetails.Should().BeEquivalentTo(userDetails);
        }
    }
}