using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.Subscriptions.Commands
{
    public class WhenHandlingRenewSubscriptionKeyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Api_Called(
            RenewSubscriptionKeyCommand command,
            [Frozen] Mock<IApiClient> mockApiClient,
            RenewSubscriptionKeyCommandHandler handler)
        {
            var expectedPostRequest = new PostRenewSubscriptionKeyRequest(command.AccountIdentifier, command.ProductId);
            mockApiClient.Setup(c =>
                    c.Post<string>(
                        It.Is<PostRenewSubscriptionKeyRequest>(x =>
                            x.PostUrl.Equals(expectedPostRequest.PostUrl))))
                .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.NoContent, ""));

            var actual = await handler.Handle(command, CancellationToken.None);
            
            actual.Should().Be(Unit.Value);
            mockApiClient.Verify(client => client.Post<string>(
                    It.Is<PostRenewSubscriptionKeyRequest>(x =>
                        x.PostUrl.Equals(expectedPostRequest.PostUrl))),
                Times.Once);
        }
    }
}