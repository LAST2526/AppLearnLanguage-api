
using Amazon;
using Autofac;
using Hangfire;
using Last02.Data;
using Last02.Models;
using Last02.Services.DI;
using Last02.Services.Implement;
using Last02.Services.Interfaces;
using Last02.Web.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    AWSConfigs.LoggingConfig.LogTo = LoggingOptions.Console;
    AWSConfigs.LoggingConfig.LogResponses = ResponseLoggingOption.Always;
    AWSConfigs.LoggingConfig.LogMetrics = true;

    // Add services to the container.
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddSessionStateTempDataProvider();

    var connectionString = builder.Configuration.GetConnectionString("Last02Connection") ?? throw new InvalidOperationException("Connection string 'Last02Connection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.Configure<IISOptions>(options =>
    {
        options.AutomaticAuthentication = false;
    });
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

    builder.Services
        .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login";
            options.AccessDeniedPath = "/Login";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });

    builder.Services.AddAuthorization();

    // Add dependency injection
    builder.Services.AddServiceCollection(builder.Configuration, builder.Host);
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterType<ClaimsService>()
                        .As<IClaimsService>()
                        .InstancePerLifetimeScope();

        containerBuilder.RegisterType<ClaimsService>()
                        .AsSelf()
                        .InstancePerLifetimeScope();
    });

    builder.Services.AddHttpClient();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(connectionString));
    builder.Services.AddHangfireServer();

    var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();


    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new CookieHangfireAuthorizationFilter() }
    });

    app.UseStaticFiles();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
