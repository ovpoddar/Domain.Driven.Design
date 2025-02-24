﻿using DDD.Application.Exceptions;
using DDD.Domain.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace DDD.Api.Infrastructure.Exception;

/// <summary>
/// This exception handler is registered last. For custom handlers, you can add them which will be 
/// registered automatically. these custom handlers will be registered in the order they are discovered 
/// by reflection. However, this handler is not affected by reflection discovery and is always 
/// registered last.
/// </summary>
public class GlobalExceptionErrorHandling : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionErrorHandling> _logger;

    public GlobalExceptionErrorHandling(ILogger<GlobalExceptionErrorHandling> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        System.Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        if (exception.GetType().BaseType == typeof(DomainConcernException))
        {
            var domainException = (DomainConcernException)exception;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = domainException.StatusCode;
            await httpContext.Response.WriteAsync(exception.Message, cancellationToken: cancellationToken);
        }
        else if (exception is BaseValidationException validationException)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsync(string.Join(",", validationException.Errors), cancellationToken: cancellationToken);
        }
        else
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsync("Internal error occured", cancellationToken: cancellationToken);
        }
        return true;
    }
}
