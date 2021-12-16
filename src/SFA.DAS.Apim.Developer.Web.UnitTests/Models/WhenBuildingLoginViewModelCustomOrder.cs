using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Models
{
    public class WhenBuildingLoginViewModelCustomOrder
    {
        [Test]
        public void Then_The_Fields_Are_In_The_Correct_Order()
        {
            var actual = LoginViewModel.BuildPropertyOrderDictionary();
            
            actual[nameof(LoginViewModel.EmailAddress)].Should().Be(0);
            actual[nameof(LoginViewModel.Password)].Should().Be(1);
        }    
    }
}