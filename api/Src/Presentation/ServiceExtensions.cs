using Microsoft.Extensions.DependencyInjection;

namespace DDD.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        return services;
    }
}
