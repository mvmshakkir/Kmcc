using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using demo.Areas.Identity.Data;
using demo.Models;
using demo.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
//using demo.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration.Json;
//using demo.Controllers;
using Stimulsoft.Report;
using System.IO;
using Stimulsoft.Base;

//using static Stimulsoft.Report.Func;
internal class Program
{
    public static object Configuration { get; private set; }
    //public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
    {
        Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkcgIvwL0jnpsDqRpWg5FI5kt2G7A0tYIcUygBh1sPs7koivWV0htru4Pn2682yhdY3+9jxMCVTKcKAjiEjgJzqXgLFCpe62hxJ7/VJZ9Hq5l39md0pyydqd5Dc1fSWhCtYqC042BVmGNkukYJQN0ufCozjA/qsNxzNMyEql26oHE6wWE77pHutroj+tKfOO1skJ52cbZklqPm8OiH/9mfU4rrkLffOhDQFnIxxhzhr2BL5pDFFCZ7axXX12y/4qzn5QLPBn1AVLo3NVrSmJB2KiwGwR4RL4RsYVxGScsYoCZbwqK2YrdbPHP0t5vOiLjBQ+Oy6F4rNtDYHn7SNMpthfkYiRoOibqDkPaX+RyCany0Z+uz8bzAg0oprJEn6qpkQ56WMEppdMJ9/CBnEbTFwn1s/9s8kYsmXCvtI4iQcz+RkUWspLcBzlmj0lJXWjTKMRZz+e9PmY11Au16wOnBU3NHvRc9T/Zk0YFh439GKd/fRwQrk8nJevYU65ENdAOqiP5po7Vnhif5FCiHRpxgF";
        services.AddRazorPages().AddRazorRuntimeCompilation();
        //var licenseKey = File.ReadAllText("license.key");
        // StiLicense.Key = licenseKey;
        services.Configure<IISServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });

    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var Configuration = builder.Configuration;
        var connectionString = builder.Configuration.GetConnectionString("demoContextConnection") ?? throw new InvalidOperationException("Connection string 'demoContextConnection' not found.");
        builder.Services.AddScoped<QRCodeService>(serviceProvider =>
        {
            var context = serviceProvider.GetRequiredService<demoContext>();
            var contentRootPath = serviceProvider.GetRequiredService<IWebHostEnvironment>().ContentRootPath;
            return new QRCodeService(context, contentRootPath);
        });
        //var EmailSettings = builder.Configuration.GetConnectionString("EmailSettings");
        //var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
        builder.Services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
		builder.Services.Configure<IdSettings>(builder.Configuration.GetSection("IdSettings"));

		//builder.Services.AddTransient<IFileService, FileService>();
		builder.Services.AddDbContext<demoContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDbContext<demoContext>(options =>
        options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<demoUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<demoContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourAppNameAuthCookie";
        // Other cookie options and configurations
    });



        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
        builder.Services.AddControllersWithViews().AddRazorPagesOptions(options =>
        {
            options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "login");
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var contentRootR = env.ContentRootPath;
            var licenseFile = Path.Combine(contentRootR, "Reports", "license.key");
           // Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkcgIvwL0jnpsDqRpWg5FI5kt2G7A0tYIcUygBh1sPs7koivWV0htru4Pn2682yhdY3+9jxMCVTKcKAjiEjgJzqXgLFCpe62hxJ7/VJZ9Hq5l39md0pyydqd5Dc1fSWhCtYqC042BVmGNkukYJQN0ufCozjA/qsNxzNMyEql26oHE6wWE77pHutroj+tKfOO1skJ52cbZklqPm8OiH/9mfU4rrkLffOhDQFnIxxhzhr2BL5pDFFCZ7axXX12y/4qzn5QLPBn1AVLo3NVrSmJB2KiwGwR4RL4RsYVxGScsYoCZbwqK2YrdbPHP0t5vOiLjBQ+Oy6F4rNtDYHn7SNMpthfkYiRoOibqDkPaX+RyCany0Z+uz8bzAg0oprJEn6qpkQ56WMEppdMJ9/CBnEbTFwn1s/9s8kYsmXCvtI4iQcz+RkUWspLcBzlmj0lJXWjTKMRZz+e9PmY11Au16wOnBU3NHvRc9T/Zk0YFh439GKd/fRwQrk8nJevYU65ENdAOqiP5po7Vnhif5FCiHRpxgF";
            //if (File.Exists(licenseFile))
            //{
                StiLicense.LoadFromFile(licenseFile);
            //    Console.WriteLine("Stimulsoft license loaded successfully.");
            //}
            //else
            //{
            //    Console.WriteLine($"Stimulsoft license file not found at: {licenseFile}");
            //}
            //if (env.IsDevelopment())
            //{

            //    app.UseDeveloperExceptionPage();
            //}
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        };
		

		app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication(); ;

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Home}/{id?}");
        app.MapControllerRoute(
     name: "designer",
     pattern: "{controller=ReportDesigner}/{action=Index}/{id?}");

        app.MapRazorPages();

        ConfigureStimulsoftLicense(app);
        app.Run();
    }
    private static void ConfigureStimulsoftLicense(WebApplication app)
    {
        var env = app.Services.GetRequiredService<IWebHostEnvironment>();
        var contentRootPath = env.ContentRootPath;
        var licenseFilePath = Path.Combine(contentRootPath, "Reports", "license.key");

        if (File.Exists(licenseFilePath))
        {
            Stimulsoft.Base.StiLicense.LoadFromFile(licenseFilePath);
            Console.WriteLine("Stimulsoft license loaded successfully.");
        }
        else
        {
            Console.WriteLine($"Stimulsoft license file not found at: {licenseFilePath}");
        }
    }

}