using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiInmobiliaria
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
            services.AddControllersWithViews();

            // Tutorial Mariano Luazza clase 10 : https://www.youtube.com/watch?v=4igLX11Udvk
            // Tutorial Mariano Luazza clase 12 : https://www.youtube.com/watch?v=YrVsUrNyZqE
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = "/Usuario/Login"; //a donde dirigirse para login
                    options.LogoutPath = "/Usuario/Logout"; //a donde dirigirse para logout
                    options.AccessDeniedPath = "/Home/Restringido"; //a donde dirigirse para recurso restringido
                });
            services.AddAuthorization(options =>
            {
                // Forma abreviada de agregar Policy
                options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "SuperAdministrador"));

                // Forma genérica de agregar Policy
                //options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));

                //options.AddPolicy("Usuario", policy => policy.RequireRole("SuperAdministrador", "Administrador", "Empleado", "Cliente"));

            });
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
