using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;

namespace SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public static implicit operator RegisterCommand(RegisterRequest source)
        {
            return new RegisterCommand
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