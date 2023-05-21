using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniChatApplication.Data;
using UniChatApplication.Hubs;

namespace UniChatApplication
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

            services
                .AddDbContext<UniChatDbContext>(options =>
                    options
                        .UseSqlServer(Configuration
                            .GetConnectionString("UniChatDatabase"),
                            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

            services.AddDistributedMemoryCache();

            services
                .AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromHours(24);
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });

            services.AddSignalR();
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

            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseSession();

            app
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<ChatHub>("/chatHub");

                    endpoints
                        .MapControllerRoute(name: "Logout",
                        pattern: "Login/Logout/");

                    endpoints
                        .MapControllerRoute(name: "Error",
                        pattern: "Home/Error/");

                    endpoints
                        .MapControllerRoute(name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
