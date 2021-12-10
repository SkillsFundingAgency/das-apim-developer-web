using System;
using System.Net.Mail;
using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Validation;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser
{
    public class AuthenticateUserCommandValidator : IValidator<AuthenticateUserCommand>
    {
        public Task<ValidationResult> ValidateAsync(AuthenticateUserCommand item)
        {
            var validationResult = new ValidationResult();

            try
            {
                var emailAddress = new MailAddress(item.Email);
                if (!emailAddress.Address.Equals(item.Email, StringComparison.CurrentCultureIgnoreCase))
                {
                    validationResult.AddError(nameof(item.Email),"Enter an email address in the correct format, like name@example.com");
                }
            }
            catch (FormatException)
            {
                validationResult.AddError(nameof(item.Email),"Enter an email address in the correct format, like name@example.com");
            }
            
            return Task.FromResult(validationResult);
        }
    }
}