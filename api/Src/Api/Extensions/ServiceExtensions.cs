using Serilog;
using System.Configuration;

namespace DDD.Api.Extensions;


public static class ServiceExtensions
{
    public static IServiceCollection AddApiServicesConfiguration(this IServiceCollection service, IConfiguration configuration)
    {

        return service;
    }

    /// <summary>
    /// Configure Serilog to log in to database and into output
    /// </summary>
    /// <param name="builder">Web Builder</param>
    /// <returns><see cref="WebApplicationBuilder"/></returns>
    public static WebApplicationBuilder AddDatabaseLogging(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        builder.Logging.ClearProviders()
            .AddConfiguration(configuration.GetSection("Logging") ?? throw new ApplicationException("Logging section is not found in appsettings.json file."))
            .AddConsole()
            .AddDebug();

        builder.Host.UseSerilog();

        return builder;
    }
}
