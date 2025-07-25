using Microsoft.AspNetCore.Authentication.Cookies;
using SparkServer.Infrastructure.Repositories;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.Models.Analytics;
using SparkServerLite.Repositories;

namespace SparkServerLite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register DI
            builder.Services.AddTransient<IBlogRepository<Blog>, BlogRepository>();
            builder.Services.AddTransient<IAuthorRepository<Author>, AuthorRepository>();
            builder.Services.AddTransient<IBlogTagRepository<BlogTag>, BlogTagRepository>();
            builder.Services.AddScoped<IAnalyticsRepository<Visit>, AnalyticsRepository>();
            builder.Services.AddScoped<Interfaces.ILogger, Logger>();

            // Authentication
            builder.Services.AddAuthentication(options => {

                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {

                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/Forbidden";

                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;
            });

            // Load configuration/settings from appSettings
            IAppSettings appSettings = new AppSettings();
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            config.GetSection("SparkServerLite").Bind(appSettings);
            builder.Services.AddSingleton(appSettings);

            IAppContent appContent = new AppContent();
            IConfigurationRoot content = new ConfigurationBuilder().AddJsonFile("appcontent.json").Build();
            content.GetSection("SparkServerLite").Bind(appContent);
            builder.Services.AddSingleton(appContent);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}