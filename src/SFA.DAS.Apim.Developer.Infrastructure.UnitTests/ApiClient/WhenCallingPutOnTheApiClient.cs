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
    public class WhenCallingPutOnTheApiClient
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            Guid responseId,
            PutData putContent,
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
            var putTestRequest = new PutTestRequest(id) {Data = putContent};
            var expectedUrl = config.BaseUrl + putTestRequest.PutUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(expectedUrl), config.Key, HttpMethod.Put);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new Api.ApiClient(client, configMock.Object);
            
            
            //Act
            var actual = await apiClient.Put<string>(putTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Put)
                        && c.RequestUri.AbsoluteUri.Contains(putTestRequest.PutUrl)),
                    ItExpr.IsAny<CancellationToken>()
                );
            Guid.Parse(actual.Body).Should().Be(responseId);
        }
        
        [Test, AutoData]
        public async Task Then_If_It_Is_Not_Successful_Error_Returned(
            PutData putContent,
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
            var putTestRequest = new PutTestRequest(id) {Data = putContent};
            var expectedUrl = config.BaseUrl + putTestRequest.PutUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(expectedUrl), config.Key, HttpMethod.Put);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new Api.ApiClient(client, configMock.Object);
            
            //Act
            var actual = await apiClient.Put<string>(putTestRequest);
            
            //assert
            actual.Body.Should().BeNull();
            actual.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            actual.ErrorContent.Should().Be(responseContent);
        }
        
        private class PutTestRequest : IPutApiRequest
        {
            private readonly int _id;

            public PutTestRequest (int id)
            {
                _id = id;
            }
            public object Data { get; set; }
            public string PutUrl => $"test-url/put/{_id}";
        }

        public class PutData
        {
            public string Id { get; set; }
        }
    }
}