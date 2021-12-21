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
                var emailAddress = new MailAddress(item.EmailAddress);
                if (!emailAddress.Address.Equals(item.EmailAddress, StringComparison.CurrentCultureIgnoreCase))
                {
                    validationResult.AddError(nameof(item.EmailAddress),"Enter an email address in the correct format, like name@example.com");
                }
            }
            catch (FormatException)
            {
                validationResult.AddError(nameof(item.EmailAddress),"Enter an email address in the correct format, like name@example.com");
            }
            
            return Task.FromResult(validationResult);
        }
    }
}