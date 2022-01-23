using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.ChangePassword;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.SendChangePasswordEmail;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.VerifyRegistration;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Queries.GetUser;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    [Route("third-party-accounts")]
    public class ThirdPartyAccountsController : Controller
    {
        private readonly IDataProtectorService _dataProtector;
        private readonly IMediator _mediator;
        private readonly ILogger<ThirdPartyAccountsController> _logger;

        public ThirdPartyAccountsController(
            IDataProtectorService dataProtector,
            IMediator mediator,
            ILogger<ThirdPartyAccountsController> logger)
        {
            _dataProtector = dataProtector;
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("register", Name = RouteNames.ThirdPartyRegister)]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [Route("register", Name = RouteNames.ThirdPartyRegister)]
        public async Task<IActionResult> PostRegister(RegisterRequest request)
        {
            try
            {
                var userId = Guid.NewGuid();
                var encodedId = _dataProtector.EncodedData(userId);
                var confirmEmailUrl = Url.RouteUrl(
                    RouteNames.ThirdPartyRegisterComplete,
                    new {id = encodedId},
                    Request.Scheme,
                    Request.Host.Host);
                
                var command = (RegisterCommand) request;
                command.Id = userId;
                command.ConfirmUrl = confirmEmailUrl;
                await _mediator.Send(command);
                
                return RedirectToRoute(RouteNames.ThirdPartyAwaitingConfirmEmail);
            }
            catch (ValidationException e)
            {
                foreach (var member in e.ValidationResult.MemberNames)
                {
                    var memberParts = member.Split('|');
                    ModelState.AddModelError(memberParts[0], memberParts[1]);
                }
                
                var model = (RegisterViewModel)request;
                return View("Register", model);
            }
        }
        
        [HttpGet]
        [Route("register/awaiting-confirmation", Name = RouteNames.ThirdPartyAwaitingConfirmEmail)]
        public IActionResult RegisterAwaitingConfirmEmail()
        {
            return View();
        }
        
        [HttpGet]
        [Route("register/complete", Name = RouteNames.ThirdPartyRegisterComplete)]
        public async Task<IActionResult> RegisterComplete([FromQuery]string id)
        {
            var decodedId = _dataProtector.DecodeData(id);
            if (!decodedId.HasValue)
            {
                return RedirectToRoute(RouteNames.ThirdPartyRegister);
            }

            try
            {
                await _mediator.Send(new VerifyRegistrationCommand {Id = decodedId.Value});
                return View(decodedId.Value);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, e.Message);
                return RedirectToRoute(RouteNames.ThirdPartyRegister);
            }
        }

        [HttpGet]
        [Route("sign-in", Name=RouteNames.ThirdPartySignIn)]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [Route("sign-in", Name = RouteNames.ThirdPartySignIn)]
        public async Task<IActionResult> PostLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            try
            {
                var userResponse = await _mediator.Send(new AuthenticateUserCommand
                {
                    EmailAddress = model.EmailAddress,
                    Password = model.Password
                });
                
                if (userResponse.UserDetails is { State: "blocked" })
                {
                    model.AccountIsLocked = true;
                    return View("Login", model);
                }

                if (!(userResponse.UserDetails is { Authenticated: true }))
                {
                    ModelState.AddModelError("","Invalid credentials");
                    return View("Login", model);
                }
            
                return RedirectToRoute(RouteNames.ExternalApiHub, new { externalId = userResponse.UserDetails.Id });
            }
            catch (ValidationException e)
            {
                foreach (var member in e.ValidationResult.MemberNames)
                {
                    var memberParts = member.Split('|');
                    ModelState.AddModelError(memberParts[0], memberParts[1]);
                }
                
                return View("Login", model);
            }
        }
        
        [Route("logout", Name = RouteNames.ExternalLogout)]
        public IActionResult ExternalSignOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToRoute(RouteNames.Index);
        }
        
        [HttpGet]
        [Route("forgotten-password", Name = RouteNames.ThirdPartyForgottenPassword)]
        public IActionResult ForgottenPassword()
        {
            return View();
        }
        
        [HttpPost]
        [Route("forgotten-password", Name = RouteNames.ThirdPartyForgottenPassword)]
        public async Task<IActionResult> PostForgottenPassword(ForgottenPasswordViewModel model)
        {
            try
            {
                var user = await _mediator.Send(new GetUserQuery {EmailAddress = model.EmailAddress});

                if (user.User == null)
                {
                    return RedirectToRoute(RouteNames.ThirdPartyForgottenPasswordComplete);
                }
                
                var encodedId = _dataProtector.EncodedData(Guid.Parse(user.User.Id));
                var changePasswordUrl = Url.RouteUrl(
                    RouteNames.ThirdPartyChangePassword,
                    new {id = encodedId},
                    Request.Scheme,
                    Request.Host.Host);

                await _mediator.Send(new SendChangePasswordEmailCommand
                {
                    Id = Guid.Parse(user.User.Id),
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    Email = user.User.Email,
                    ChangePasswordUrl = changePasswordUrl
                });

                return RedirectToRoute(RouteNames.ThirdPartyForgottenPasswordComplete);
            }
            catch (ValidationException e)
            {
                foreach (var member in e.ValidationResult.MemberNames)
                {
                    var memberParts = member.Split('|');
                    ModelState.AddModelError(memberParts[0], memberParts[1]);
                }
                
                return View("ForgottenPassword", model);
            }
        }
        
        [HttpGet]
        [Route("forgotten-password/complete", Name = RouteNames.ThirdPartyForgottenPasswordComplete)]
        public IActionResult ForgottenPasswordComplete()
        {
            return View();
        }
        
        [HttpGet]
        [Route("change-password", Name = RouteNames.ThirdPartyChangePassword)]
        public IActionResult ChangePassword([FromQuery]string id)
        {
            var decodedId = _dataProtector.DecodeData(id);
            if (!decodedId.HasValue)
            {
                return RedirectToRoute(RouteNames.ThirdPartyRegister);
            }

            return View(new ChangePasswordViewModel {Id = decodedId.Value});
        }
        
        [HttpPost]
        [Route("change-password", Name = RouteNames.ThirdPartyChangePassword)]
        public async Task<IActionResult> PostChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                await _mediator.Send(new ChangePasswordCommand
                {
                    Id = model.Id,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                });

                return RedirectToRoute(RouteNames.ThirdPartyChangePasswordComplete);
            }
            catch (ValidationException e)
            {
                foreach (var member in e.ValidationResult.MemberNames)
                {
                    var memberParts = member.Split('|');
                    ModelState.AddModelError(memberParts[0], memberParts[1]);
                }
                
                return View("ChangePassword", model);
            }
        }
        
        [HttpGet]
        [Route("change-password/complete", Name = RouteNames.ThirdPartyChangePasswordComplete)]
        public IActionResult ChangePasswordComplete()
        {
            return View();
        }
    }
}