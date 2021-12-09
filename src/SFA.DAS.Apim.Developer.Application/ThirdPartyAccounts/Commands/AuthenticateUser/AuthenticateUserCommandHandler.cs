using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserCommandResponse>
    {
        private readonly IUserService _userService;

        public AuthenticateUserCommandHandler (IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<AuthenticateUserCommandResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.AuthenticateUser(request.Email, request.Password);

            return new AuthenticateUserCommandResponse
            {
                UserDetails = user
            };
        }
    }
}