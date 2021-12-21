using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.ThirdPartyAccounts.Requests
{
    public class WhenBuildingPutActivateThirdPartyUserRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(Guid id)
        {
            var actual = new PutActivateThirdPartyUserRequest(id);

            actual.PutUrl.Should().Be($"users/{id}/activate");
        }
    }
}