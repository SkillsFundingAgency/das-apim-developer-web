using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.SendChangePasswordEmail
{
    public class SendChangePasswordEmailCommandHandler : IRequestHandler<SendChangePasswordEmailCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public SendChangePasswordEmailCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<Unit> Handle(SendChangePasswordEmailCommand request, CancellationToken cancellationToken)
        {
            var data = new PostSendChangePasswordEmailRequestData(request);
            var apiResponse = await _apiClient.Post<object>(new PostSendChangePasswordEmailRequest(data));
            
            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                throw new InvalidOperationException(apiResponse.ErrorContent);
            }
            
            return Unit.Value;
        }
    }
}