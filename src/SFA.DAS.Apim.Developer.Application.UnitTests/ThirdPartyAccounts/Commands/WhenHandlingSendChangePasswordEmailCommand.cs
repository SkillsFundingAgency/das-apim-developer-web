using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.SendChangePasswordEmail;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Commands
{
    public class WhenHandlingSendChangePasswordEmailCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Response_Returned(
            SendChangePasswordEmailCommand command,
            [Frozen] Mock<IApiClient> mockApiClient,
            SendChangePasswordEmailCommandHandler handler)
        {
            var data = new PostSendChangePasswordEmailRequestData(command);
            var expectedPostRequest = new PostSendChangePasswordEmailRequest(data);
            mockApiClient
                .Setup(client => client.Post<string>(It.Is<PostSendChangePasswordEmailRequest>(request =>
                    request.PostUrl.Equals(expectedPostRequest.PostUrl))))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.NoContent, ""));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().Be(Unit.Value);
            mockApiClient.Verify(client => client.Post<string>(
                    It.Is<PostSendChangePasswordEmailRequest>(x =>
                        x.PostUrl.Equals(expectedPostRequest.PostUrl))),
                Times.Once);
        }
        
        [Test, MoqAutoData]
        public void And_Error_From_Api_Then_Throws_Exception(
            SendChangePasswordEmailCommand command,
            string errorContent,
            [Frozen] Mock<IApiClient> mockApiClient,
            SendChangePasswordEmailCommandHandler handler)
        {
            mockApiClient
                .Setup(client => client.Post<string>(It.IsAny<PostSendChangePasswordEmailRequest>()))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.InternalServerError, errorContent));

            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            act.Should().Throw<InvalidOperationException>()
                .WithMessage($"*{errorContent}*");
        }
    }
}