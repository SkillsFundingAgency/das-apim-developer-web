using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.AuthenticateUser;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.VerifyRegistration;
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
    }
}