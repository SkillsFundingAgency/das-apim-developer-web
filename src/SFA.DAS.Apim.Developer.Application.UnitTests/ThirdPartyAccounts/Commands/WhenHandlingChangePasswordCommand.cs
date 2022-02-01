using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.Apim.Developer.Domain.Validation.ValidationResult;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenHandlingChangePasswordCommand
    {
        [Test, MoqAutoData]
        public void And_Command_Invalid_Then_Throws_ValidationException(
            string propertyName,
            ChangePasswordCommand command,
            ValidationResult validationResult,
            [Frozen] Mock<IValidator<ChangePasswordCommand>> mockValidator,
            ChangePasswordCommandHandler handler)
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
            ChangePasswordCommand command,
            [Frozen] Mock<IValidator<ChangePasswordCommand>> mockValidator,
            [Frozen] Mock<IApiClient> mockApiClient,
            ChangePasswordCommandHandler handler)
        {
            var expectedRequest = new PutChangePasswordRequest(command.Id, command.Password);
            mockValidator
                .Setup(validator => validator.ValidateAsync(command))
                .ReturnsAsync(new ValidationResult());
            mockApiClient
                .Setup(client => client.Put<string>(It.Is<PutChangePasswordRequest>(request =>
                    request.PutUrl.Equals(expectedRequest.PutUrl))))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.Created, ""));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().Be(Unit.Value);
            mockApiClient.Verify(client => client.Put<string>(
                    It.Is<PutChangePasswordRequest>(x =>
                        x.PutUrl.Equals(expectedRequest.PutUrl))),
                Times.Once);
        }
        
        [Test, MoqAutoData]
        public void And_Error_From_Api_Then_Throws_Exception(
            ChangePasswordCommand command,
            string errorContent,
            [Frozen] Mock<IValidator<ChangePasswordCommand>> mockValidator,
            [Frozen] Mock<IApiClient> mockApiClient,
            ChangePasswordCommandHandler handler)
        {
            mockValidator
                .Setup(validator => validator.ValidateAsync(command))
                .ReturnsAsync(new ValidationResult());
            mockApiClient
                .Setup(client => client.Put<string>(It.IsAny<PutChangePasswordRequest>()))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.InternalServerError, errorContent));

            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            act.Should().Throw<ValidationException>()
                .WithMessage($"*Error|There was a problem changing your password.*");
        }
    }
}