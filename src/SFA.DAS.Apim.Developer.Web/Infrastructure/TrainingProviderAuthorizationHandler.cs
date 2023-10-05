using Microsoft.AspNetCore.Authorization;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Web.Infrastructure
{
    /// <summary>
    /// Interface to define contracts related to Training Provider Authorization Handlers.
    /// </summary>
    public interface ITrainingProviderAuthorizationHandler
    {
        /// <summary>
        /// Contract to check is the logged in Provider is a valid Training Provider. 
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext.</param>
        /// <param name="allowAllUserRoles">boolean.</param>
        /// <returns>boolean.</returns>
        Task<bool> IsProviderAuthorized(AuthorizationHandlerContext context, bool allowAllUserRoles);
    }

    ///<inheritdoc cref="ITrainingProviderAuthorizationHandler"/>
    public class TrainingProviderAuthorizationHandler : ITrainingProviderAuthorizationHandler
    {
        private readonly ITrainingProviderService _trainingProviderService;

        public TrainingProviderAuthorizationHandler(
            ITrainingProviderService trainingProviderService)
        { 
            _trainingProviderService = trainingProviderService;
        }

        public async Task<bool> IsProviderAuthorized(AuthorizationHandlerContext context, bool allowAllUserRoles)
        {
            var ukprn = GetProviderId(context);

            //if the ukprn is invalid return false.
            if (ukprn <= 0) return false;

            var providerStatusDetails = await _trainingProviderService.GetProviderStatus(ukprn);

            // Condition to check if the Provider Details has permission to access Apprenticeship Services based on the property value "CanAccessApprenticeshipService" set to True.
            return providerStatusDetails is { CanAccessService: true };
        }

        private static long GetProviderId(AuthorizationHandlerContext context)
        {
            return long.TryParse(context.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn))?.Value, out var providerId)
                ? providerId
                : 0;
        }
    }
}
