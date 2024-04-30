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
using demo.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration.Json;
using demo.Controllers;
internal class Program
{
    public static object Configuration { get; private set; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages().AddRazorRuntimeCompilation();
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
        builder.Services.AddControllersWithViews().AddRazorPagesOptions(options => {
            options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
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
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }
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
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();


        app.Run();
    }
}