using System;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.SendChangePasswordEmail
{
    public class SendChangePasswordEmailCommand : IRequest<Unit>, ISendChangePasswordEmailData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ChangePasswordUrl { get; set; }
    }
}