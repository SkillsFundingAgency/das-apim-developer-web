using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.UnitTests.HttpMessageHandlerMock;

namespace SFA.DAS.Apim.Developer.Infrastructure.UnitTests.ApiClient
{
    public class WhenCallingPostOnTheApiClient
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            Guid responseId,
            PostData postContent,
            int id,
            ApimDeveloperApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}/";
            var configMock = new Mock<IOptions<ApimDeveloperApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                Content = new StringContent($"'{responseId}'"),
                StatusCode = HttpStatusCode.Accepted
            };
            var postTestRequest = new PostTestRequest(id) {Data = postContent};
            var expectedUrl = config.BaseUrl + postTestRequest.PostUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(expectedUrl), config.Key, HttpMethod.Post);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new Api.ApiClient(client, configMock.Object);
            
            
            //Act
            var actual = await apiClient.Post<string>(postTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Post)
                        && c.RequestUri.AbsoluteUri.Contains(postTestRequest.PostUrl)),
                    ItExpr.IsAny<CancellationToken>()
                );
            Guid.Parse(actual.Body).Should().Be(responseId);
        }
        
        [Test, AutoData]
        public async Task Then_If_It_Is_Not_Successful_Error_Returned(
            PostData postContent,
            int id,
            string responseContent,
            ApimDeveloperApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}/";
            var configMock = new Mock<IOptions<ApimDeveloperApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(responseContent)
            };
            var postTestRequest = new PostTestRequest(id) {Data = postContent};
            var expectedUrl = config.BaseUrl + postTestRequest.PostUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(expectedUrl), config.Key, HttpMethod.Post);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new Api.ApiClient(client, configMock.Object);
            
            //Act
            var actual = await apiClient.Post<string>(postTestRequest);
            
            //assert
            actual.Body.Should().BeNull();
            actual.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            actual.ErrorContent.Should().Be(responseContent);
        }
        
        private class PostTestRequest : IPostApiRequest
        {
            private readonly int _id;

            public PostTestRequest (int id)
            {
                _id = id;
            }
            public object Data { get; set; }
            public string PostUrl => $"test-url/post/{_id}";
        }

        public class PostData
        {
            public string Id { get; set; }
        }
    }
}