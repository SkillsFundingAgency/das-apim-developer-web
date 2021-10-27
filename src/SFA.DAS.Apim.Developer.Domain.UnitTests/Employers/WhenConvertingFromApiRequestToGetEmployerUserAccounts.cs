using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Domain.Employers.Api;
using SFA.DAS.Apim.Developer.Domain.Employers.Api.Responses;

namespace SFA.DAS.Apim.Developer.Domain.UnitTests.Employers
{
    public class WhenConvertingFromApiRequestToGetEmployerUserAccounts
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped(GetUserAccountsResponse source)
        {
            var actual = (GetEmployerUserAccounts) source;

            actual.EmployerAccounts.Should().BeEquivalentTo(source.UserAccounts);
        }
    }
}