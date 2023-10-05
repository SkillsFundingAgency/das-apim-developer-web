using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.Provider.Services;
using SFA.DAS.Apim.Developer.Domain.Api;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Providers.Api.Requests;
using SFA.DAS.Apim.Developer.Domain.Providers.Api.Responses;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.Provider.Services
{
    public class TrainingProviderServiceTest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Made_And_ProviderResponse_Returned(
            long ukprn,
            ApiResponse<ProviderAccountResponse> apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            TrainingProviderService service)
        {
            //Arrange
            var request = new GetProviderStatusDetails(ukprn);
            apiClient.Setup(x =>
                    x.Get<ProviderAccountResponse>(
                        It.Is<GetProviderStatusDetails>(c => c.GetUrl.Equals(request.GetUrl))))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await service.GetProviderStatus(ukprn);

            //Assert
            actual.CanAccessService.Should().Be(apiResponse.Body.CanAccessService);
        }
    }
}
