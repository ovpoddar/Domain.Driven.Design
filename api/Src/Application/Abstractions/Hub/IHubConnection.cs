using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Abstractions.Hub;

public interface IHubConnection<T> where T : class
{
    IHubClients<T> Clients { get; }
    IGroupManager Groups { get; }
}
