using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.ThirdPartyAccounts
{
    public class WhenMappingToAuthenticateUserDetailsFromPostAuthenticateUserResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(PostAuthenticateUserResponseItem source)
        {
            //Act
            var actual = (AuthenticateUserDetails)source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
        
        [Test, AutoData]
        public void And_Source_Null_Then_Returns_Null()
        {
            //arrange
            PostAuthenticateUserResponseItem source = null;
            //Act
            var actual = (AuthenticateUserDetails)source;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}