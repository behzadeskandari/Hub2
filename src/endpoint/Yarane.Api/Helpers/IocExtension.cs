using System.Diagnostics;
using CleanLog;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using Yarane.Application.Services;
using Yarane.Common.Utilities;
using Yarane.Contract.Dtos.Settings;
using Yarane.Contract.Interfaces.ProxyServices.Dotin;
using Yarane.Contract.Interfaces.ProxyServices.OmidSystem;
using Yarane.Contract.Interfaces.UserService;
using Yarane.Persistance.Contexts;
using Yarane.Persistance.Security;
using Yarane.Proxy.Services.Dotin;
using Yarane.Proxy.Services.OmidSystem;

namespace Yarane.Api.Helpers;

public static class IocExtension
{
    public static void AddRedis(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var publishConfig = configuration.GetSection("PublishConfig").Get<PublishConfig>();
        string connectionConfig = Convert.ToBoolean(publishConfig.IsProduction)
            ? "ConnectigConfigProduction"
            : "ConnectigConfigDevelopment";
        var connectigConfig = configuration.GetSection(connectionConfig).Get<ConnectigConfig>();


        builder.Services.AddStackExchangeRedisCache(options =>
        {
            var _congiguration = builder.Configuration;
            var configurationOptions = ConfigurationOptions.Parse(connectigConfig.RedisIp);
            configurationOptions.DefaultDatabase = Convert.ToInt16(connectigConfig.DatabaseNumber);

            options.ConfigurationOptions = configurationOptions;
        });
    }
    public static void AddDatabase(this IServiceCollection services, WebApplicationBuilder builder)
    {

        var configuration = builder.Configuration;
        var publishConfig = configuration.GetSection("PublishConfig").Get<PublishConfig>();
        string connectionConfig = Convert.ToBoolean(publishConfig.IsProduction)
            ? "ConnectigConfigProduction"
            : "ConnectigConfigDevelopment";
        var connectigConfig = configuration.GetSection(connectionConfig).Get<ConnectigConfig>();

        services.AddDbContext<YaraneDbContext>(options => options.UseSqlServer(connectigConfig.ConnectionString));

        builder.Services.AddDbContext<ReportDbContext>(options =>
        {
            options.UseOracle(builder.Configuration.GetConnectionString("ReportConnection"));
        });
        //services.AddDbContext<YaraneDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("YaraneConStr")));
    }
    public static void FillSiteSetting(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddOptions<SiteSetting>().Bind(builder.Configuration.GetSection("DotinConfig"));
        services.AddOptions<SiteSetting>().Bind(builder.Configuration.GetSection("ConnectigConfigDevelopment"));
        services.AddOptions<SiteSetting>().Bind(builder.Configuration.GetSection("ConnectigConfigProduction"));
        services.AddOptions<SiteSetting>().Bind(builder.Configuration.GetSection("PublishConfig"));
        services.AddOptions();
        services.Configure<SiteSetting>(options => builder.Configuration.Bind(options));
    }
    public static void AddStopwatch(this IServiceCollection services)
    {
        services.AddTransient<Stopwatch>();
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        //services.AddScoped<IStatementRepository, StatementRepository>();

    }
    public static void AddQueues(this IServiceCollection services)
    {
        //services.AddScoped<IQueuePublisher, QueuePublisher>();
    }
    public static void AddUtilityHelper(this IServiceCollection services)
    {
        services.AddSingleton<Helper>();
    }
    public static void AddUnitOfWork(this IServiceCollection services)
    {
     
    }
    public static void AddCustomServices(this IServiceCollection services)
    {

        services.AddScoped<SiteSetting>();

        services.AddScoped<IPasswordHelper, PasswordHelper>();

        //services.AddSingleton<CaptchaService>();
    }
    public static void AddProxies(this IServiceCollection services)
    {
        services.AddScoped<IDotinProxyServices, DotinProxyServices>();
        services.AddScoped<IOmidSystemProxyServices, OmidSystemProxyServices>();
    }
    public static void AddHttpClientFactory(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        var PublishConfig = configuration.GetSection("PublishConfig").Get<PublishConfig>();
        string connectionConfig = Convert.ToBoolean(PublishConfig.IsProduction) == true ? "ConnectigConfigProduction" : "ConnectigConfigDevelopment";
        var ConnectigConfig = configuration.GetSection(connectionConfig).Get<DotinConfig>();
        services.AddHttpClient("Yarane", c =>
        {
            c.BaseAddress = new Uri(ConnectigConfig.ServiceUrl + "NIX/Urbanism/");
            c.Timeout = TimeSpan.FromSeconds(100);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, sslPolicyErrors) => true
        });
        services.AddHttpClient("dotin", c =>
        {
            c.BaseAddress = new Uri(ConnectigConfig.ServiceUrl);
            c.Timeout = TimeSpan.FromSeconds(100);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, sslPolicyErrors) => true
        });
        services.AddHttpClient("omidSystem", c =>
        {
            c.BaseAddress = new Uri(ConnectigConfig.ServiceUrl);
            c.Timeout = TimeSpan.FromSeconds(100);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, sslPolicyErrors) => true
        });
    }

    public static void AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(
            opt =>
            {
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                new HeaderApiVersionReader("x-api-version"),
                                                                new MediaTypeApiVersionReader("x-api-version"));
            }
        );
    }
    public static void AddHangfireSchaduler(this IServiceCollection services)
    {
        //services.AddHangfire(config =>
        //{
        //    config.UseSqlServerStorage("Data Source= .; Initial Catalog = BackGroundTasksDb; Integrated Security = SSPI; MultipleActiveResultSets=True; TrustServerCertificate=True;");
        //});
        //services.AddHangfireServer();
    }
    public static void AddLogLibrary(this IServiceCollection services)
    {
        services.AddCleanLog();
    }

    public static void AddMiddlewares(this WebApplication app)
    {
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        //app.UseCleanLog();

        //app.UseCustomExceptionHandler();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();


    }
}
