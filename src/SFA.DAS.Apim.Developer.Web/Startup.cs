using System;
using System.IO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Apim.Developer.Application.Subscriptions.Queries.GetAvailableProducts;
using SFA.DAS.Apim.Developer.Infrastructure.Configuration;
using SFA.DAS.Apim.Developer.Web.Infrastructure.Configuration;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Apim.Developer.Web.Extensions;
using SFA.DAS.Apim.Developer.Web.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.Provider.Shared.UI.Startup;

namespace SFA.DAS.Apim.Developer.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfigurationRoot _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory());
#if DEBUG
            if (!configuration.IsDev())
            {
                config.AddJsonFile("appsettings.json", false)
                    .AddJsonFile("appsettings.Development.json", true);
            }
#endif
            
            config.AddEnvironmentVariables();
            if (!configuration.IsDev())
            {
                config.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                        options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                        options.EnvironmentName = configuration["Environment"];
                        options.PreFixConfigurationKeys = false;
                    }
                );
            }
            _configuration = config.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddOptions();
            var serviceParameters = new ServiceParameters();
            if (_configuration["AuthType"].Equals("Employer", StringComparison.CurrentCultureIgnoreCase))
            {
                serviceParameters.AuthenticationType = AuthenticationType.Employer;
            }
            else if (_configuration["AuthType"].Equals("Provider", StringComparison.CurrentCultureIgnoreCase))
            {
                serviceParameters.AuthenticationType = AuthenticationType.Provider;
            }
            else if (_configuration["AuthType"].Equals("External", StringComparison.CurrentCultureIgnoreCase))
            {
                serviceParameters.AuthenticationType = AuthenticationType.External;
            }

            services.AddConfigurationOptions(_configuration, serviceParameters.AuthenticationType);
            
            
            if (serviceParameters.AuthenticationType == AuthenticationType.Employer)
            {
                var clientId = "no-auth-id";
                services.AddEmployerAuthenticationServices();
                if (_configuration["ApimDeveloperWeb:UseGovSignIn"] != null && _configuration["ApimDeveloperWeb:UseGovSignIn"]
                        .Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    services.AddAndConfigureGovUkAuthentication(_configuration, $"{typeof(AddServiceRegistrationExtension).Assembly.GetName().Name}.Auth",typeof(EmployerAccountPostAuthenticationClaimsHandler));
                }
                else
                {
                     if (_configuration["StubAuth"] != null && _configuration["StubAuth"]
                             .Equals("true", StringComparison.CurrentCultureIgnoreCase))
                     {
                         services.AddEmployerStubAuthentication();    
                     }
                     else
                     {
                         var config = _configuration
                             .GetSection("Identity")
                             .Get<IdentityServerConfiguration>();
                         services.AddAndConfigureEmployerAuthentication(config);
                         clientId = config.ClientId;
                     }
                     services.AddAuthenticationCookie(serviceParameters.AuthenticationType);
                }

                services.AddMaMenuConfiguration(RouteNames.EmployerSignOut, clientId,_configuration["ResourceEnvironmentName"]);
                services.Configure<ExternalLinksConfiguration>(_configuration.GetSection(ExternalLinksConfiguration.ApimDeveloperExternalLinksConfiguration));
                services.AddSingleton(new ProviderSharedUIConfiguration());
            }
            else if (serviceParameters.AuthenticationType == AuthenticationType.Provider)
            {
                services.AddProviderUiServiceRegistration(_configuration);
                services.AddProviderAuthenticationServices();
                if (_configuration["StubAuth"] != null && _configuration["StubAuth"]
                        .Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    services.AddProviderStubAuthentication();
                }
                else
                {
                    services.AddAndConfigureProviderAuthentication(_configuration
                        .GetSection(nameof(ProviderIdams))
                        .Get<ProviderIdams>());    
                }
                services.AddAuthenticationCookie(serviceParameters.AuthenticationType);    
            }
            else if (serviceParameters.AuthenticationType == AuthenticationType.External)
            {
                services.AddExternalAuthenticationServices();
                if (_configuration["StubAuth"] != null && _configuration["StubAuth"]
                        .Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    services.AddExternalStubAuthentication();
                }
                else
                {
                    services.AddAndConfigureExternalUserAuthentication();    
                }
                
                services.AddSingleton(new ProviderSharedUIConfiguration());
                services.AddAuthenticationCookie(serviceParameters.AuthenticationType);
            }

            services.AddSharedAuthenticationServices();
            
            services.AddMediatR(typeof(GetAvailableProductsQuery).Assembly);
            services.AddMediatRValidation();
            services.AddServiceRegistration(serviceParameters, _configuration);
            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
            
            services.Configure<RouteOptions>(options =>
            {
                
            }).AddMvc(options =>
                {
                    if (!_configuration.IsDev())
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());    
                    }
                    
                })
                .EnableGoogleAnalytics();
            services.AddAuthorizationService(serviceParameters.AuthenticationType);

            services.AddApplicationInsightsTelemetry();

            if (!_environment.IsDevelopment())
            {
                services.AddHealthChecks();
                services.AddDataProtection(_configuration);
            }
#if DEBUG
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHealthChecks();
                app.UseExceptionHandler("/Error/500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            
            app.Use(async (context, next) =>
            {
                if (context.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.Response.Headers.Remove("X-Frame-Options");
                }

                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                await next();

                if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    var originalPath = context.Request.Path.Value;
                    context.Items["originalPath"] = originalPath;
                    context.Request.Path = "/error/404";
                    await next();
                }
            });

            app.UseRouting();
            
            app.UseAuthorization();
            
            app.UseEndpoints(builder =>
            {
                builder.MapDefaultControllerRoute();
            });

        }
    }
}
