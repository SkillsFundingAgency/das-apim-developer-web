using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}