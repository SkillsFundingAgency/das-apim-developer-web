using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.DeleteSubscriptionKey;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.Subscriptions.Commands
{
    public class WhenHandlingDeleteSubscriptionKeyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Api_Called(
            DeleteSubscriptionKeyCommand command,
            [Frozen] Mock<IApiClient> mockApiClient,
            DeleteSubscriptionKeyCommandHandler handler)
        {
            var expectedPostRequest = new DeleteSubscriptionKeyRequest(command.AccountIdentifier, command.ProductId);
            mockApiClient.Setup(c =>
                    c.Delete<string>(
                        It.Is<DeleteSubscriptionKeyRequest>(x =>
                            x.DeleteUrl.Equals(expectedPostRequest.DeleteUrl))))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.Created, ""));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().Be(Unit.Value);
            mockApiClient.Verify(client => client.Delete<string>(
                    It.Is<DeleteSubscriptionKeyRequest>(x =>
                        x.DeleteUrl.Equals(expectedPostRequest.DeleteUrl))),
                Times.Once);
        }
    }
}