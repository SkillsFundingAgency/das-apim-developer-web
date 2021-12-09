using MediatR;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser
{
    public class AuthenticateUserCommand : IRequest<AuthenticateUserCommandResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}