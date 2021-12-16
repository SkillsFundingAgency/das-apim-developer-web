﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Developer.Web.Infrastructure;

namespace SFA.DAS.Apim.Developer.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("",Name = RouteNames.Index)]

        public IActionResult Index()
        {
            return View();
        }
    }
}