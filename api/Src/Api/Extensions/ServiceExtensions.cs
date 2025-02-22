﻿using DDD.Domain.Infrastructure;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Configuration;

namespace DDD.Api.Extensions;


public static class ServiceExtensions
{
    public static IServiceCollection AddApiServicesConfiguration(this IServiceCollection service, IConfiguration configuration)
    {
        var origin = configuration.GetSection("App").GetValue<string>("UiApplicationUrl");
        if (!string.IsNullOrWhiteSpace(origin) && origin.Split(',').Length > 0)
        {
            var origins = origin.Split(',');
            service.AddCors(o =>
            {
                o.AddDefaultPolicy(p => p.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod());

                o.AddPolicy(CrossOriginConstants.AllowHostsOnly, p => p.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod());

                o.AddPolicy(CrossOriginConstants.AllowAllRequests, p => p.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

                o.AddPolicy(CrossOriginConstants.AllowAnyPostRequests, p => p.WithOrigins(origins)
                    .AllowAnyHeader()
                    .WithMethods("Post"));
            });
        }

        return service;
    }

    /// <summary>
    /// Configures Serilog to log events to a SQL Server database and optionally to other outputs.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <param name="isOnlyLogger">If true, Serilog will be the only logger; otherwise, it will be added to the existing logging providers.</param>
    /// <returns>The updated <see cref="WebApplicationBuilder"/>.</returns>
    /// <exception cref="ApplicationException">Thrown if the connection string is not found in the configuration.</exception>
    public static WebApplicationBuilder AddDatabaseLogging(this WebApplicationBuilder builder, bool isOnlyLogger = true)
    {

        var connectionString = builder.Configuration.GetConnectionString("msSQLDbConnection")
            ?? throw new ApplicationException("msSQLDbConnection connection propriety is not found in appsettings.json file.");
        var sink = new MSSqlServerSinkOptions()
        {
            TableName = "Logging",
            AutoCreateSqlTable = true
        };
        var columnOptions = new ColumnOptions();
        columnOptions.Store.Remove(StandardColumn.TimeStamp);
        columnOptions.Store.Add(StandardColumn.LogEvent);
        columnOptions.LogEvent.DataLength = 2048;
        var logger = new LoggerConfiguration()
            .WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: sink,
                columnOptions: columnOptions);

        if (isOnlyLogger) builder.Host.UseSerilog((_, configuration) => configuration = logger);
        else builder.Logging.AddSerilog(logger.CreateLogger());
        return builder;

    }

}
