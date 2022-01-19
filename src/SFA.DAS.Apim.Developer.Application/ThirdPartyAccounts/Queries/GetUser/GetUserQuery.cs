using MediatR;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Responses;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserResponse>
    {
        public string EmailAddress { get; set; }
    }
}