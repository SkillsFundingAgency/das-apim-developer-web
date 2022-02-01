using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IValidator<ChangePasswordCommand> _validator;
        private readonly IApiClient _apiClient;

        public ChangePasswordCommandHandler(IValidator<ChangePasswordCommand> validator, IApiClient apiClient)
        {
            _validator = validator;
            _apiClient = apiClient;
        }
        
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }
            
            var apiResponse = await _apiClient.Put<string>(new PutChangePasswordRequest(request.Id, request.Password));
            
            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                validationResult.AddError("Error", "There was a problem changing your password.");
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }
            
            return Unit.Value;
        }
    }
}