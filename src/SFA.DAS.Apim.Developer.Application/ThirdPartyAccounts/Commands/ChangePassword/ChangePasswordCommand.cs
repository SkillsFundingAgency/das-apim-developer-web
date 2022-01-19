using System;
using MediatR;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}