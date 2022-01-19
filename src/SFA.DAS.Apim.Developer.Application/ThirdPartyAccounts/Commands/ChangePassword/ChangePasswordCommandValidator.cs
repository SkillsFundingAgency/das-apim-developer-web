using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Validation;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : IValidator<ChangePasswordCommand>
    {
        public Task<ValidationResult> ValidateAsync(ChangePasswordCommand item)
        {
            var validationResult = new ValidationResult();
            
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
            
            return Task.FromResult(validationResult);
        }
    }
}