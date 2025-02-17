using EComm.Data.Interfaces;
using EComm.EF;
using EComm.Web.Api.gRPC;
using EComm.Web.Controllers;
using EComm.Web.Interfaces;
using EComm.Web.Policies;
using EComm.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace EComm.Web
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
            services.AddGrpc();
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.LoginPath = "/login"; });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminsOnly", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });

                options.AddPolicy("LessThan100", policy =>
                {
                    policy.AddRequirements(new ProductLessThanOneHundredRequirement());
                });

                options.AddPolicy("LessThan", policy =>
                {
                    policy.AddRequirements(new ProductLessThanRequirement(25.00m));
                });
            });
            services.AddDbContext<ECommDataContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("ECommConnection")));
            services.AddScoped<IRepository, ECommDataContext>(
                sp => sp.GetService<ECommDataContext>());
            services.AddSingleton<IEmailService, EmailService>();
            services.AddTransient<IEmailFormatter, ImportantEmailFormatter>();
            services.AddScoped<IAuthorizationHandler, ProductLessThanOneHundredHandler>();
            services.AddScoped<IAuthorizationHandler, ProductLessThanHandler>();
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/clienterror", "?statuscode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<ECommGrpcService>();
            });
        
        }
    }
}
