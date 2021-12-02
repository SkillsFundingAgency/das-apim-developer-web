using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResult>
    {
        private readonly IValidator<RegisterCommand> _validator;

        public RegisterCommandHandler(IValidator<RegisterCommand> validator)
        {
            _validator = validator;
        }
        
        public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }
            
            return new RegisterCommandResult();
        }
    }
}