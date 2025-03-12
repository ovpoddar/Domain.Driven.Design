using DDD.Application.Abstractions.Hub;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Presentation.Construct;

public class SignalRConnection<T, TJ> : IHubConnection<T> where T : class where TJ : Hub<T>
{
    private readonly IHubContext<TJ, T> _context;

    public SignalRConnection(IHubContext<TJ, T> context) => 
        _context = context;

    public IHubClients<T> Clients => _context.Clients;

    public IGroupManager Groups => _context.Groups;
}