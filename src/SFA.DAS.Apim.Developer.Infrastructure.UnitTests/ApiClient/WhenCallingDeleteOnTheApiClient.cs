using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Infrastructure.UnitTests.HttpMessageHandlerMock;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Apim.Developer.Infrastructure.UnitTests.ApiClient
{
    public class WhenCallingDeleteOnTheApiClient
    {

        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header_And_Data_Returned(
            ApimDeveloperApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<ApimDeveloperApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var delTestRequest = new DeleteTestRequest();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + delTestRequest.DeleteUrl), config.Key, HttpMethod.Delete);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new Api.ApiClient(client, configMock.Object);

            //Act
            var actual = await apiClient.Delete<string>(delTestRequest);

            //Assert
            actual.StatusCode.Equals(HttpStatusCode.Accepted);
        }

        [Test, AutoData]
        public async Task Then_If_It_Is_Not_Successful_An_Error_Is_Returned(
            ApimDeveloperApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<ApimDeveloperApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var delTestRequest = new DeleteTestRequest();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + delTestRequest.DeleteUrl), config.Key, HttpMethod.Delete);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new Api.ApiClient(client, configMock.Object);

            //Act Assert
            var actual = await apiClient.Delete<string>(delTestRequest);
            actual.StatusCode.Equals(HttpStatusCode.BadRequest);
            actual.Body.Should().BeNull();

        }

        [Test, AutoData]
        public async Task Then_If_It_Is_Not_Found_Default_Is_Returned(
            ApimDeveloperApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}";
            var configMock = new Mock<IOptions<ApimDeveloperApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var delTestRequest = new DeleteTestRequest();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(config.BaseUrl + delTestRequest.DeleteUrl), config.Key, HttpMethod.Delete);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new Api.ApiClient(client, configMock.Object);

            //Act Assert
            var actual = await apiClient.Delete<string>(delTestRequest);

            actual.Body.Should().BeNull();
            actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private class DeleteTestRequest : IDeleteApiRequest
        {
            public string DeleteUrl => $"/test-url/del";
        }
    }
}