using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheRichLifeProject.interfaces;
using TheRichLifeProject.Models;
using TheRichLifeProject.Repositories;

namespace TheRichLifeProject
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
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => WishList.GetList(sp));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                       options =>
                       {
                           options.LoginPath = new PathString("/Login/Index");
                           options.AccessDeniedPath = new PathString("/Login/Denied");
                       });
            services.AddDistributedMemoryCache();
            services.AddOptions();
            services.AddSession(options =>
            {
                // the session timeout, the users session is saved for this amount of time as long as the browser stays open
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
            });
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
            services.AddSingleton(_ => Configuration);
            services.AddDbContext<DatabaseContext>(options => options.UseMySql(Configuration.GetConnectionString("DatabaseContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
