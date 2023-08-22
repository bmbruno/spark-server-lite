using SparkServer.Infrastructure.Repositories;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;

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

            // Load configuration/settings from appSettings
            IAppSettings appSettings = new AppSettings();
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            config.GetSection("SparkServerLite").Bind(appSettings);
            builder.Services.AddSingleton(appSettings);

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}