using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.ThirdPartyAccounts.Requests
{
    public class WhenBuildingAPutChangePasswordRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            Guid id,
            string password)
        {
            //Act
            var actual = new PutChangePasswordRequest(id, password);
            
            //Assert
            actual.PutUrl.Should().Be($"users/{id}/change-password");
            ((PutChangePasswordRequestData)actual.Data).Password.Should().BeEquivalentTo(password);
        }
    }
}