namespace SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts
{
    public class ForgottenPasswordViewModel
    {
        public string EmailAddress { get; set; }
    }
    
    public class ChangePasswordViewModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}