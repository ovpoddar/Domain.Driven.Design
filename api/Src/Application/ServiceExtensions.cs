using DDD.Application.Abstractions.Behaviors;
using FluentValidation;
﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(typeof(IAssemblyMarker).Assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        return service;
    }
}
