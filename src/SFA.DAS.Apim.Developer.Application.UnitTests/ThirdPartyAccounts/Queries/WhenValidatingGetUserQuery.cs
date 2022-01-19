using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Queries.GetUser;

namespace SFA.DAS.Apim.Developer.Application.UnitTests.ThirdPartyAccounts.Queries
{
    public class WhenValidatingGetUserQuery
    {
        [Test, AutoData]
        public async Task And_All_Fields_Present_Then_Valid(
            GetUserQuery query,
            GetUserQueryValidator validator)
        {
            //Arrange
            query.EmailAddress = $"{query.EmailAddress}@email.com";
            
            //Act
            var actual = await validator.ValidateAsync(query);

            //Assert
            actual.IsValid().Should().BeTrue();
        }
        
        [Test, AutoData]
        public async Task And_No_EmailAddress_Then_Not_Valid(
            GetUserQuery query,
            GetUserQueryValidator validator)
        {
            //Arrange
            query.EmailAddress = null;
            
            //Act
            var actual = await validator.ValidateAsync(query);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(query.EmailAddress));
            actual.ValidationDictionary[nameof(query.EmailAddress)].Should()
                .Be("Enter an email address");
        }
        
        [Test, AutoData]
        public async Task And_MalFormed_EmailAddress_Then_Not_Valid(
            GetUserQuery query,
            GetUserQueryValidator validator)
        {
            //Act
            var actual = await validator.ValidateAsync(query);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(query.EmailAddress));
            actual.ValidationDictionary[nameof(query.EmailAddress)].Should()
                .Be("Enter an email address in the correct format, like name@example.com");
        }
    }
}