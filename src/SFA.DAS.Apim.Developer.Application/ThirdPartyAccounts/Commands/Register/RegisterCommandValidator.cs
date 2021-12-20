using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Validation;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register
{
    public class RegisterCommandValidator : IValidator<RegisterCommand>
    {
        public Task<ValidationResult> ValidateAsync(RegisterCommand item)
        {
            var validationResult = new ValidationResult();
            
            if (string.IsNullOrEmpty(item.FirstName))
            {
                validationResult.AddError(nameof(item.FirstName), "Enter first name");
            }
            if (string.IsNullOrEmpty(item.LastName))
            {
                validationResult.AddError(nameof(item.LastName), "Enter last name");
            }
            if (string.IsNullOrEmpty(item.EmailAddress))
            {
                validationResult.AddError(nameof(item.EmailAddress), "Enter an email address");
            }
            else
            {
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
            }
            if (string.IsNullOrEmpty(item.Password))
            {
                validationResult.AddError(nameof(item.Password), "Enter a password");
            }
            else
            {
                var regex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$";
                if (!Regex.IsMatch(item.Password, regex))
                {
                    validationResult.AddError(nameof(item.Password),"Password must contain upper and lowercase letters, a number and at least 8 characters");
                }
            }
            if (string.IsNullOrEmpty(item.ConfirmPassword))
            {
                validationResult.AddError(nameof(item.ConfirmPassword), "Re-type password");
            }
            else
            {
                if (item.Password != item.ConfirmPassword)
                {
                    validationResult.AddError(nameof(item.ConfirmPassword), "Passwords do not match");
                }
            }

            return Task.FromResult(validationResult);
        }
    }
}