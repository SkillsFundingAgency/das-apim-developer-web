using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.ThirdPartyAccounts
{
    public class WhenMappingToUserDetailsFromGetUserResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetUserResponseItem source)
        {
            //Act
            var actual = (UserDetails)source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}