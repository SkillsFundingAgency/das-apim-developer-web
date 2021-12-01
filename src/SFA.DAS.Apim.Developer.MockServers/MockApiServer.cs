﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WireMock.Logging;
using WireMock.Matchers;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;

namespace SFA.DAS.Apim.Developer.MockServer
{
    public static class MockApiServer
    {
        public static IWireMockServer Start()
        {
            var settings = new WireMockServerSettings
            {
                Port = 5031,
                Logger = new WireMockConsoleLogger()
            };
            var server = StandAloneApp.Start(settings);

            server.Given(Request.Create()
                .WithPath(s => Regex.IsMatch(s, "/subscriptions/products/([A-Za-z0-9])+"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("product-subscriptions.json"));

            server.Given(Request.Create()
                .WithPath(s => Regex.IsMatch(s, "/accountusers/\\d+/accounts"))
                //http:localhost:5031/accountusers/45464/accounts
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("user-accounts.json"));


            return server;
        }
    }
}
