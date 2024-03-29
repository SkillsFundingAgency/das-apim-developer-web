﻿using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Queries.GetUser;
using SFA.DAS.Apim.Developer.Domain.Interfaces;

namespace SFA.DAS.Apim.Developer.Web.AppStart
{
    public static class MediatRExtensions
    {
        public static void AddMediatRValidation(this IServiceCollection services)
        {
            services.AddScoped(typeof(IValidator<RegisterCommand>), typeof(RegisterCommandValidator));
            services.AddScoped(typeof(IValidator<AuthenticateUserCommand>), typeof(AuthenticateUserCommandValidator));
            services.AddScoped(typeof(IValidator<GetUserQuery>), typeof(GetUserQueryValidator));
            services.AddScoped(typeof(IValidator<ChangePasswordCommand>), typeof(ChangePasswordCommandValidator));
        }
    }
}