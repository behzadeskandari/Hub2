using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Serilog;
using ServiceAggregator.Api.Services;
using Yarane.Api.Helpers;
using Yarane.Application.Services;
using Yarane.Contract.Interfaces.UserService;
using Yarane.Domain.Entities;
using Yarane.Persistance.Contexts;

// ── Serilog bootstrap ──────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File("logs/FanavaranHub-.log", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    Console.WriteLine($"Content Root: {builder.Environment.ContentRootPath}");
    // ── Serilog ─────────────────────────────────────────────────────
    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "Yarane.Api")
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
        .WriteTo.File("logs/identityserver-.log", rollingInterval: RollingInterval.Day));


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("EnableCors", builder =>
        {

            builder.WithOrigins(
                "http://192.168.140.104:8071", "http://192.168.140.104:8070",
                "http://localhost:8071", "http://192.168.140.104:8070",
                "http://maskanmelli.banksepah.net", "https://maskanmelli.banksepah.net",
                "http://maskanmelli.banksepah.net:8070", "https://maskanmelli.banksepah.net/8070",
                "http://maskanmelli.banksepah.net:8071", "https://maskanmelli.banksepah.net/8071",
                "http://10.43.1.33", "https://10.43.1.33",
                "http://10.43.1.54:80", "https://10.43.1.54:80",
                "http://localhost:5052", "https://localhost:5052",
                "http://10.43.1.54", "https://10.43.1.54",
                "http://localhost", "https://localhost",
                "http://localhost:5239", "https://localhost:5239",
                "http://localhost:8080", "http://localhost:8080",
                "http://localhost:8081", "http://localhost:8081",
                "http://localhost:8083", "http://localhost:8083",
                "http://localhost:8085", "http://localhost:8085",
                "http://localhost:4200",
                "http://192.168.13.119:8080", "http://192.168.13.119:8081", "http://192.168.13.119:8082",
                "https://192.168.13.119:8080", "https://192.168.13.119:8081", "https://192.168.13.119:8082",
                "http://192.168.140.104:8080", "https://192.168.140.104:8080"
                )
                .AllowCredentials()// This is necessary for sending cookies or auth tokens
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    builder.Services.AddControllers();
    //    .AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    //});
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSingleton<SkiaSharpCaptchaService>();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "NehzatBack - 1404/02/29",
            Version = "1.0.27",
        });
    });

    builder.Services.AddMemoryCache();
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.AddServerHeader = false; // Removes 'Server: Kestrel'
    });

    builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = false; // Disables 'api-supported-versions' header
    });

    builder.Services.AddIdentityCore<Users>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = true;

    })
    .AddRoles<IdentityRole>() //adds roles 
    .AddRoleManager<RoleManager<IdentityRole>>() //be able to use of role manager 
    .AddEntityFrameworkStores<YaraneDbContext>() //providing our context 
    .AddSignInManager<SignInManager<Users>>() ////make use of sigin manager 
    .AddUserManager<UserManager<Users>>() //make use of usemanager  to create user
    .AddDefaultTokenProviders(); //be abe to create tokens for email confimation 

   

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

    });

    // Add IocExtensions
    builder.Services.FillSiteSetting(builder);
    builder.Services.AddDatabase(builder);
    builder.Services.AddRedis(builder);
    builder.Services.AddStopwatch();
    builder.Services.AddRepositories();
    builder.Services.AddQueues();
    builder.Services.AddUtilityHelper();
    builder.Services.AddUnitOfWork();
    builder.Services.AddCustomServices();
    builder.Services.AddProxies();
    builder.Services.AddHttpClientFactory(builder.Configuration);
    builder.Services.AddCustomApiVersioning();
    builder.Services.AddHangfireSchaduler();
    builder.Services.AddLogLibrary();


    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IJwtTokenGenerator, JwtService>();
    builder.Services.AddScoped<IReportService, ReportService>();
    //add for capthcha
    //builder.Services.AddSession(options =>
    //{
    //    options.IdleTimeout = TimeSpan.FromMinutes(1);
    //    options.Cookie.HttpOnly = true;
    //    options.Cookie.IsEssential = true;
    //});
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "behzad",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("F9z!4@8vP#c2wQk5g7*XjS3&nMdYt6Lb"))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // If Authorization header is empty, read from cookie
                var tokenFromCookie = context.Request.Cookies["AuthCookie"];
                if (!string.IsNullOrEmpty(tokenFromCookie))
                {
                    context.Token = tokenFromCookie;
                }

                return Task.CompletedTask;
            }
        };
    });

    var app = builder.Build();

    app.UseCors("EnableCors");
    app.AddMiddlewares();

    //app.UseSession();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseDeveloperExceptionPage();

    //AppRoles.RoleManagers(app.Services);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Identity server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}



public static class AppRoles
{
    public static void RoleManagers(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<YaraneDbContext>();

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new IdentityRole[]
                {
                        new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                        new IdentityRole() { Name = "User", ConcurrencyStamp = "1", NormalizedName = "User" },
                        new IdentityRole() { Name = "Staff", ConcurrencyStamp = "1", NormalizedName = "Staff" },
                });
                 context.SaveChangesAsync();
            }
        }
    }
}
