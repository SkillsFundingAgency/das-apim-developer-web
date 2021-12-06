using System;
using MediatR;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterCommandResult>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ConfirmUrl { get; set; }
    }
}