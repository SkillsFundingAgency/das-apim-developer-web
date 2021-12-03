using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Subscriptions.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.Subscriptions.Commands
{
    public class WhenHandlingCreateSubscriptionKeyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Post_Request_Made_To_Create_Subscription(
            CreateSubscriptionKeyCommand command,
            [Frozen] Mock<IApiClient> apiClient,
            CreateSubscriptionKeyCommandHandler handler)
        {
            var expectedPostCreateSubscription =
                new PostCreateSubscriptionRequest(command.AccountIdentifier, command.ProductId, command.AccountType);
            apiClient.Setup(x =>
                    x.Post<object>(It.Is<PostCreateSubscriptionRequest>(c =>
                        c.PostUrl.Equals(expectedPostCreateSubscription.PostUrl))))
                .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.Created, ""));
            
            var actual = await handler.Handle(command, CancellationToken.None);
            
            actual.Should().Be(Unit.Value);
            apiClient.Verify(client => client.Post<object>(
                    It.Is<PostCreateSubscriptionRequest>(x =>
                        x.PostUrl.Equals(expectedPostCreateSubscription.PostUrl))),
                Times.Once);

        }
    }
}