using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.Validation;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterCommandResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    
    public class RegisterCommandValidator : IValidator<RegisterCommand>
    {
        public Task<ValidationResult> ValidateAsync(RegisterCommand item)
        {
            var validationResult = new ValidationResult();

            return Task.FromResult(validationResult);
        }
    }
}