using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace DDD.Application.Exceptions;

public static class ExceptionHandlerRegister
{
    public static void RegisterHandlers(this IServiceCollection service, Assembly assembly)
    {
        var handlers = assembly.GetTypes()
            .Where(a => a.IsClass
            && a.GetInterfaces().Contains(typeof(IExceptionHandler)));

        foreach (var handler in handlers)
            service.AddSingleton(typeof(IExceptionHandler), handler);
    }

}
