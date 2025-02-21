using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application;


public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        return service;
    }
}
