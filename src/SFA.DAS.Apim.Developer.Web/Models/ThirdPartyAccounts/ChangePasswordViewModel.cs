using System;

namespace SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts
{
    public class ChangePasswordViewModel
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}