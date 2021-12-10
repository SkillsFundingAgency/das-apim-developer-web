using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IValidator<AuthenticateUserCommand> _validator;

        public AuthenticateUserCommandHandler (IUserService userService, IValidator<AuthenticateUserCommand> validator)
        {
            _userService = userService;
            _validator = validator;
        }
        
        public async Task<AuthenticateUserCommandResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }
            
            var user = await _userService.AuthenticateUser(request.Email, request.Password);

            return new AuthenticateUserCommandResponse
            {
                UserDetails = user
            };
        }
    }
}