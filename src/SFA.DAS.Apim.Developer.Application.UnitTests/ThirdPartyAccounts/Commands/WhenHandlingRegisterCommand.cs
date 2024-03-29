﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
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
            
            act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Response_Returned(
            RegisterCommand command,
            [Frozen] Mock<IValidator<RegisterCommand>> mockValidator,
            [Frozen] Mock<IApiClient> mockApiClient,
            RegisterCommandHandler handler)
        {
            var data = new PostRegisterThirdPartyAccountData(command);
            var expectedPostRequest = new PostRegisterThirdPartyAccountRequest(data);
            mockValidator
                .Setup(validator => validator.ValidateAsync(command))
                .ReturnsAsync(new ValidationResult());
            mockApiClient
                .Setup(client => client.Post<string>(It.Is<PostRegisterThirdPartyAccountRequest>(request =>
                    request.PostUrl.Equals(expectedPostRequest.PostUrl))))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.Created, ""));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().Be(Unit.Value);
            mockApiClient.Verify(client => client.Post<string>(
                    It.Is<PostRegisterThirdPartyAccountRequest>(x =>
                        x.PostUrl.Equals(expectedPostRequest.PostUrl))),
                Times.Once);
        }
        
        [Test, MoqAutoData]
        public void And_Error_From_Api_Then_Throws_Exception(
            RegisterCommand command,
            string errorContent,
            [Frozen] Mock<IValidator<RegisterCommand>> mockValidator,
            [Frozen] Mock<IApiClient> mockApiClient,
            RegisterCommandHandler handler)
        {
            var data = new PostRegisterThirdPartyAccountData(command);
            var expectedPostRequest = new PostRegisterThirdPartyAccountRequest(data);
            mockValidator
                .Setup(validator => validator.ValidateAsync(command))
                .ReturnsAsync(new ValidationResult());
            mockApiClient
                .Setup(client => client.Post<string>(It.Is<PostRegisterThirdPartyAccountRequest>(request =>
                    request.PostUrl.Equals(expectedPostRequest.PostUrl))))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.InternalServerError, errorContent));

            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*Error|There was a problem creating your account. If you have already registered then check for a confirmation email or sign in.*");
        }
    }
}