using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.VerifyRegistration;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenHandlingVerifyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Response_Returned(
            VerifyRegistrationCommand command,
            [Frozen] Mock<IApiClient> mockApiClient,
            VerifyRegistrationCommandHandler handler)
        {
            var expectedPutRequest = new PutActivateThirdPartyUserRequest(command.Id);
            mockApiClient
                .Setup(client => client.Put<string>(It.Is<PutActivateThirdPartyUserRequest>(request =>
                    request.PutUrl.Equals(expectedPutRequest.PutUrl))))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.NoContent, ""));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().Be(Unit.Value);
            mockApiClient.Verify(client => client.Put<string>(
                    It.Is<PutActivateThirdPartyUserRequest>(x =>
                        x.PutUrl.Equals(expectedPutRequest.PutUrl))),
                Times.Once);
        }
        
        [Test, MoqAutoData]
        public void And_Error_From_Api_Then_Throws_Exception(
            VerifyRegistrationCommand command,
            string errorContent,
            [Frozen] Mock<IApiClient> mockApiClient,
            VerifyRegistrationCommandHandler handler)
        {
            var expectedPostRequest = new PutActivateThirdPartyUserRequest(command.Id);
            mockApiClient
                .Setup(client => client.Put<string>(It.IsAny<PutActivateThirdPartyUserRequest>()))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.InternalServerError, errorContent));

            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            act.Should().Throw<InvalidOperationException>()
                .WithMessage($"*{errorContent}*");
        }
    }
}