using System;
using MediatR;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.VerifyRegistration
{
    public class VerifyRegistrationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}