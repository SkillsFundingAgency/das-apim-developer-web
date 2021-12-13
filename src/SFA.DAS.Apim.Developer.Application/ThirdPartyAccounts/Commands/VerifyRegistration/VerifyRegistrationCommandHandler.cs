using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.VerifyRegistration
{
    public class VerifyRegistrationCommandHandler : IRequestHandler<VerifyRegistrationCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public VerifyRegistrationCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<Unit> Handle(VerifyRegistrationCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new PutActivateThirdPartyUserRequest(request.Id);
            var apiResponse = await _apiClient.Put<string>(apiRequest);

            if (apiResponse.StatusCode != HttpStatusCode.NoContent)
            {
                throw new InvalidOperationException(apiResponse.ErrorContent);
            }

            return Unit.Value;
        }
    }
}