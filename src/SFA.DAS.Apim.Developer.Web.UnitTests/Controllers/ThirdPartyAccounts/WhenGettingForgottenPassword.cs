﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.Apim.Developer.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Apim.Developer.Web.UnitTests.Controllers.ThirdPartyAccounts
{
    public class WhenGettingForgottenPassword
    {
        [Test, MoqAutoData]
        public void Then_View_Returned(
            [Greedy] ThirdPartyAccountsController controller)
        {
            var actual = controller.ForgottenPassword() as ViewResult;
            
            actual!.Model.Should().BeNull();
        }
    }
}