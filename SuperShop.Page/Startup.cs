using LazZiya.ExpressLocalization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Page.LocalizationResources;
using SuperShop.Page.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SuperShop.ViewModels.Systems.User;

namespace SuperShop.Page
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Login/Login";
                    option.AccessDeniedPath = "/Login/Forbidden";
                });

            var cultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("vi")
        };
            services.AddControllersWithViews()
                     .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>())
                     .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
                     {
                         // When using all the culture providers, the localization process will
                         // check all available culture providers in order to detect the request culture.
                         // If the request culture is found it will stop checking and do localization accordingly.
                         // If the request culture is not found it will check the next provider by order.
                         // If no culture is detected the default culture will be used.

                         // Checking order for request culture:
                         // 1) RouteSegmentCultureProvider
                         //      e.g. http://localhost:1234/tr
                         // 2) QueryStringCultureProvider
                         //      e.g. http://localhost:1234/?culture=tr
                         // 3) CookieCultureProvider
                         //      Determines the culture information for a request via the value of a cookie.
                         // 4) AcceptedLanguageHeaderRequestCultureProvider
                         //      Determines the culture information for a request via the value of the Accept-Language header.
                         //      See the browsers language settings

                         // Uncomment and set to true to use only route culture provider
                         ops.UseAllCultureProviders = false;
                         ops.ResourcesPath = "LocalizationResources";
                         ops.RequestLocalizationOptions = o =>
                         {
                             o.SupportedCultures = cultures;
                             o.SupportedUICultures = cultures;
                             o.DefaultRequestCulture = new RequestCulture("en");
                         };
                     });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserApiClient, UserApiClient>();
            services.AddTransient<IProductApiClient, ProductApiClient>();
            services.AddTransient<ICategoryApiClient, CategoryApiClient>();
            services.AddTransient<ILanguageApiClient, LanguageApiClient>();
            IMvcBuilder builder = services.AddRazorPages();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

#if DEBUG
            if (environment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=en}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}