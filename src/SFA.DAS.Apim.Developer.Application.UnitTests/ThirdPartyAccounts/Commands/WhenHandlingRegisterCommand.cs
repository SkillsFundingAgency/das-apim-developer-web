using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.Apim.Developer.Domain.Validation.ValidationResult;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenHandlingRegisterCommand
    {
        [Test, MoqAutoData]
        public void And_Command_Invalid_Then_Throws_ValidationException(
            string propertyName,
            RegisterCommand command,
            ValidationResult validationResult,
            [Frozen] Mock<IValidator<RegisterCommand>> mockValidator,
            RegisterCommandHandler handler)
        {
            validationResult.AddError(propertyName, "error");
            mockValidator
                .Setup(validator => validator.ValidateAsync(command))
                .ReturnsAsync(validationResult);
            
            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));
            
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Response_Returned(
            RegisterCommand command,
            [Frozen] Mock<IValidator<RegisterCommand>> mockValidator,
            RegisterCommandHandler handler)
        {
            mockValidator
                .Setup(validator => validator.ValidateAsync(command))
                .ReturnsAsync(new ValidationResult());

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().NotBeNull();
        }
    }
}