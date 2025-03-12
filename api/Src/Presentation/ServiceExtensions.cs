using DDD.Application.Abstractions.Hub;
using DDD.Presentation.Construct;
using DDD.Presentation.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace DDD.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSignalR();
        services.RegisterHubDependency();
        return services;
    }


    public static void UseHubs(this IEndpointRouteBuilder host)
    {
        host.MapHub<WeatherForecastHub>("/weatherforecasthub");
    }

    private static void RegisterHubDependency(this IServiceCollection services)
    {
        var hubs = typeof(IAssemblyMarker).Assembly
            .GetTypes()
            .Where(a => a.IsClass
                && a.BaseType != null
                && a.BaseType.IsGenericType
                && a.BaseType.GetGenericTypeDefinition() == typeof(Hub<>)
            );


        foreach (var hub in hubs)
        {
            var interfaceType = hub.BaseType!
                .GetGenericArguments()
                .First();
            var hubConnectionInterface = typeof(IHubConnection<>)
                .MakeGenericType(interfaceType);
            var connectionType = typeof(SignalRConnection<,>)
                .MakeGenericType(interfaceType, hub);
            services.AddScoped(hubConnectionInterface, connectionType);
        }
    }
}
