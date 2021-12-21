using System;
using System.ComponentModel.DataAnnotations;
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
using ValidationResult = SFA.DAS.Apim.Developer.Domain.Validation.ValidationResult;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenHandlingAuthenticateCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Response_Returned(
            AuthenticateUserCommand command,
            UserDetails userDetails,
            [Frozen] Mock<IValidator<AuthenticateUserCommand>> mockValidator,
            [Frozen] Mock<IUserService> userService,
            AuthenticateUserCommandHandler handler)
        {
            //Arrange
            mockValidator.Setup(x => x.ValidateAsync(command)).ReturnsAsync(new ValidationResult { });
            userService.Setup(x => x.AuthenticateUser(command.EmailAddress, command.Password)).ReturnsAsync(userDetails);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.UserDetails.Should().BeEquivalentTo(userDetails);
        }

        [Test, MoqAutoData]
        public void Then_If_Not_Valid_Exception_Returned(
            string propertyName,
            AuthenticateUserCommand command,
            ValidationResult validationResult,
            [Frozen] Mock<IValidator<AuthenticateUserCommand>> mockValidator,
            AuthenticateUserCommandHandler handler)
        {
            validationResult.AddError(propertyName, "error");
            mockValidator
                .Setup(validator => validator.ValidateAsync(command))
                .ReturnsAsync(validationResult);
            
            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));
            
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }
    }
}