using MediatR;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser
{
    public class AuthenticateUserCommand : IRequest<AuthenticateUserCommandResponse>
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}