using DDD.Application.Abstractions.Database;
using DDD.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace DDD.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        var sqlConnectionString = configuration.GetConnectionString("msSQLDb")
            ?? throw new ApplicationException("msSQLDb Connection not found.");
        service.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(sqlConnectionString, a => a.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
        );
        service.AddScoped(typeof(IDatabaseConnectionBase<>), typeof(DatabaseConnectionBase<>));
        service.RegisterRepositories();

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
