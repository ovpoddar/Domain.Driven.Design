using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Infrastructure.Exceptions;

/// <summary>
/// For Domain Related issue inherit you exception class from this base class
/// this will put your costume message in data field and set the status code
/// to <see cref="HttpStatusCode.BadRequest"/> you can also update the status 
/// code to your liking
/// </summary>
public abstract class DomainConcernException : Exception
{
    public DomainConcernException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
    {
        StatusCode = (int)statusCode;
    }

    public int StatusCode { get; }
}
