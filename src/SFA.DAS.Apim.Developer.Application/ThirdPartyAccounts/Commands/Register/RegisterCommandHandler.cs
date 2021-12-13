using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Developer.Domain.Interfaces;
using SFA.DAS.Apim.Developer.Domain.ThirdPartyAccounts.Api.Requests;

namespace SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Unit>
    {
        private readonly IValidator<RegisterCommand> _validator;
        private readonly IApiClient _apiClient;

        public RegisterCommandHandler(
            IValidator<RegisterCommand> validator,
            IApiClient apiClient)
        {
            _validator = validator;
            _apiClient = apiClient;
        }
        
        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }

            var data = new PostRegisterThirdPartyAccountData(request);
            var apiResponse = await _apiClient.Post<string>(new PostRegisterThirdPartyAccountRequest(data));

            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                validationResult.AddError("Error", "There was a problem creating your account. If you have already registered then check for a confirmation email or sign in.");
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }
            
            return Unit.Value;
        }
    }
}