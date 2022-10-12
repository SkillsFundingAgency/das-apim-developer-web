using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Employer.Services;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.Employer.Services
{
    public class WhenGettingUserAccounts
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Made_And_Accounts_Returned(
            string userId,
            string email,
            ApiResponse<GetUserAccountsResponse> apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            EmployerAccountService service)
        {
            //Arrange
            var request = new GetUserAccountsRequest(userId, email);
            apiClient.Setup(x =>
                x.Get<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetUrl.Equals(request.GetUrl))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetUserAccounts(userId, email);
            
            //Assert
            actual.EmployerAccounts.Should().BeEquivalentTo(apiResponse.Body.UserAccounts);
        }
    }
}