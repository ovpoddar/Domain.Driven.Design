using DDD.Application.Abstractions.Database;
using DDD.Application.Abstractions.Redis;
using DDD.Application.Exceptions;
using DDD.Infrastructure.Configuration;
using DDD.Infrastructure.Database;
using DDD.Infrastructure.Redis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Diagnostics;

namespace DDD.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration, string? migrationAssemblyName = null)
    {
        var sqlConnectionString = configuration.GetConnectionString("msSQLDbConnection")
            ?? throw new ApplicationException("msSQLDbConnection connection propriety is not found in appsettings.json file.");
        service.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseSqlServer(sqlConnectionString,
                string.IsNullOrWhiteSpace(migrationAssemblyName)
                    ? null
                    : a => a.MigrationsAssembly(migrationAssemblyName));
        });
        service.AddScoped(typeof(IDatabaseConnectionBase<>), typeof(DatabaseConnectionBase<>));
        service.RegisterRepositories();

        var redisConnectionString = configuration.GetConnectionString("RedisConnection")
            ?? throw new ApplicationException("RedisConnection connection propriety is not found in appsettings.json file.");
        service.Configure<DistributedCacheEntryOptions>(configuration.GetConfiguration("Redis"));
        service.AddSingleton<IConnectionMultiplexer>(m => ConnectionMultiplexer.Connect(redisConnectionString));
        service.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = redisConnectionString;
        });
        service.AddScoped<IRedisCache, RedisCache>();
        service.RegisterHandlers(typeof(IAssemblyMarker).Assembly);


        service.AddIdentityApiEndpoints<IdentityUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        return service;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        var interfaces = typeof(Domain.IAssemblyMarker).Assembly
            .GetTypes()
            .Where(a =>
                a.IsInterface
                && a.Name.StartsWith('I')
                && a.Name.EndsWith("Repository")).ToArray();
        var implementations = typeof(IAssemblyMarker).Assembly
            .GetTypes()
            .Where(a =>
                a.Name.EndsWith("Repository")
                && a.IsClass)
            .ToArray();

        Debug.Assert(interfaces.Length == implementations.Length, "All the repository doesn't have an implementation. make sure you map your implementation with its interface.");

        Type actualInterface;
        foreach (var implementation in implementations)
        {
            actualInterface = interfaces.FirstOrDefault(a => a.Name == $"I{implementation.Name}")
                ?? throw new ApplicationException($"No implementation found for I{implementation.Name}. if you already have a implementation for this then make sure it follows the naming convention. like for ITestRepository interface the implementation should be TestRepository");
            services.AddTransient(actualInterface, implementation);
        }

        return services;
    }

}
