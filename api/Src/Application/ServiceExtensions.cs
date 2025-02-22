using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection service, IConfiguration configuration)
    {
        return service;
    }
}
