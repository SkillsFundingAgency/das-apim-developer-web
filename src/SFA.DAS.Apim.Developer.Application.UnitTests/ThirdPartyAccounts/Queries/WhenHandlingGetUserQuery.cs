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
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Queries.GetUser;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Queries
{
    public class WhenHandlingGetUserQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Response_Returned(
            GetUserQuery command,
            GetUserResponse apiResponse,
            [Frozen] Mock<IApiClient> mockApiClient,
            GetUserQueryHandler handler)
        {
            var expectedGetUserRequest = new GetUserRequest(command.EmailAddress);
            mockApiClient
                .Setup(client => client.Get<GetUserResponse>(It.Is<GetUserRequest>(request =>
                    request.GetUrl.Equals(expectedGetUserRequest.GetUrl))))
                .ReturnsAsync(new ApiResponse<GetUserResponse>(apiResponse, HttpStatusCode.Created, ""));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task And_Error_From_Api_Then_Returns_Null(
            GetUserQuery command,
            string errorContent,
            [Frozen] Mock<IApiClient> mockApiClient,
            GetUserQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetUserResponse>(It.IsAny<GetUserRequest>()))
                .ReturnsAsync(new ApiResponse<GetUserResponse>(null, HttpStatusCode.InternalServerError, errorContent));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().BeEquivalentTo(new GetUserResponse());
        }
    }
}