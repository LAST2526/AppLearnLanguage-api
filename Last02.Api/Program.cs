
using FluentValidation;
using FluentValidation.AspNetCore;
using Last02.Api;
using Last02.Api.Filters;
using Last02.Data;
using Last02.Data.Entities;
using Last02.Models;
using Last02.Services.DI;
using Last02.Services.Implement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelFilter>();
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Last02 API",
        Version = "v1",
        Description = "API documentation for Last02 application",
    });

    // Add XML documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token as format: {token}"
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] {}
        }
    };

    options.AddSecurityRequirement(securityRequirement);

    options.OperationFilter<AcceptLanguageHeaderOperationFilter>();
});

services.AddOpenApi();

// Cấu hình kết nối DB
var connectionString = builder.Configuration.GetConnectionString("Last02Connection")
                       ?? throw new InvalidOperationException("Connection string 'Last02Connection' not found.");
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(60);
    }));

// Cấu hình Identity
services.AddIdentity<Users, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cấu hình Services
services.Configure<GoogleAuthSettings>(builder.Configuration.GetSection("Google"));
services.AddHttpClient();
services.AddMemoryCache();
services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();

// Add settings config
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<GoogleAuthSettings>(builder.Configuration.GetSection("GoogleAuthSettings"));

// Disable default automatic model state validation
services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//Setting JWT
services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var keyString = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing in configuration.");
var key = Encoding.UTF8.GetBytes(keyString);
builder.Services.AddAuthentication(options => 
{
   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
   options.RequireHttpsMetadata = true;
   options.SaveToken = true;
   options.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuer = true,
       ValidateAudience = true,
       ValidateLifetime = true,
       ValidateIssuerSigningKey = true,
       ValidIssuer = jwtSettings["Issuer"],
       ValidAudience = jwtSettings["Audience"],
       IssuerSigningKey = new SymmetricSecurityKey(key)
   };
});

services.AddLocalization(options => options.ResourcesPath = "Resources");
ValidatorOptions.Global.LanguageManager = new CustomLanguageManager();
var supportedCultures = new[] { new CultureInfo("vi"), new CultureInfo("en"), new CultureInfo("ja") };
services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
});

//Port web backend
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5149);
});
services.AddHttpContextAccessor();
services.AddServiceCollection(builder.Configuration, builder.Host);


var app = builder.Build();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
//    RequestPath = "/api/files",
//    ServeUnknownFileTypes = true,
//    DefaultContentType = "application/octet-stream"
//});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();
//}

app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.MapControllers();

app.Run();
