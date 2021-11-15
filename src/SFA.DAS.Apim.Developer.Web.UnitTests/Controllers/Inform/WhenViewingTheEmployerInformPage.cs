using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.Inform
{
    public class WhenViewingTheEmployerInformPage
    {
        [Test, MoqAutoData]
        public void Then_The_View_Is_Returned_And_Model_Data_Passed(
            string accountId,
            [Greedy]InformController controller)
        {
            var actual = controller.Index(accountId) as ViewResult;

            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("Index");
            var actualModel = actual.Model as string;
            actualModel.Should().Be(accountId);
        }
    }
}