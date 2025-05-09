using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Apim.Developer.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Inform;

public class WhenViewingTheInformPage
{
    [Test, MoqAutoData]
    public void Then_The_View_Is_Returned_And_Model_Data_Passed_For_Employer(
        string accountId,
        ApimDeveloperWeb developerConfig,
        [Frozen] Mock<IOptions<ApimDeveloperWeb>> config,
        [Greedy]InformController controller)
    {
        config.Setup(x => x.Value).Returns(developerConfig);
        
        var actual = controller.Index(accountId) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        actual.ViewName.Should().Be("Index");
        var actualModel = actual.Model as InformViewModel;
        actualModel.EmployerAccountId.Should().Be(accountId);
        actualModel.DocumentationUrl.Should().Be(developerConfig.DocumentationBaseUrl);
    }
    
    [Test, MoqAutoData]
    public void Then_The_View_Is_Returned_And_Model_Data_Passed_For_Provider(
        int ukprn,
        ApimDeveloperWeb developerConfig,
        [Frozen] Mock<IOptions<ApimDeveloperWeb>> config,
        [Greedy]InformController controller)
    {
        config.Setup(x => x.Value).Returns(developerConfig);
        
        var actual = controller.ProviderIndex(ukprn) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        actual.ViewName.Should().Be("ProviderIndex");
        var actualModel = actual.Model as InformViewModel;
        actualModel.Ukprn.Should().Be(ukprn);
        actualModel.DocumentationUrl.Should().Be(developerConfig.DocumentationBaseUrl);
    }
}