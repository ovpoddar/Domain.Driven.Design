using DDD.Presentation.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }


    public static void UseHubs(this IEndpointRouteBuilder host)
    {
        host.MapHub<WeatherForecastHub>("/weatherforecasthub");
    }
}
