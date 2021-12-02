using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Application.ThirdPartyAccounts.Commands.Register;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    [Route("third-party-accounts")]
    public class ThirdPartyAccountsController : Controller
    {
        private readonly IMediator _mediator;

        public ThirdPartyAccountsController(IMediator mediator)
        {
            _mediator = mediator;
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
                var result = await _mediator.Send((RegisterCommand)request);
                return RedirectToRoute(RouteNames.ThirdPartyConfirmEmail, new {result.Id});
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
    }
}