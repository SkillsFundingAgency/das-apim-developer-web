using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Apim.Developer.Domain.Configuration;
using SFA.DAS.Apim.Developer.Infrastructure.Configuration;
using SFA.DAS.Apim.Developer.Web.AppStart;
using SFA.DAS.Configuration.AzureTableStorage;

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
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", false)
#endif
                .AddEnvironmentVariables();

            if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
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
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddOptions();

            var serviceParameters = new ServiceParameters
            {
                AuthenticationType = AuthenticationType.Employer
            };


            services.AddConfigurationOptions(_configuration);
            services.AddEmployerAuthenticationServices();
            
            services.AddAndConfigureEmployerAuthentication(
                _configuration
                    .GetSection("Identity")
                    .Get<IdentityServerConfiguration>());
            
            services.AddServiceRegistration(serviceParameters, _configuration);
            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            }).AddMvc(options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAuthorizationService();

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

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
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(builder =>
            {
                builder.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
