﻿namespace SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public static implicit operator RegisterViewModel(RegisterRequest source)
        {
            return new RegisterViewModel
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                EmailAddress = source.EmailAddress,
                Password = source.Password,
                ConfirmPassword = source.ConfirmPassword
            };
        }
    }
}