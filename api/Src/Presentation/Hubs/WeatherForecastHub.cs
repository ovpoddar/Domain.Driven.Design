using DDD.Application.Abstractions.Hub;
using DDD.Application.WeatherForecasts.WeatherForcastsClients.Query;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Presentation.Hubs;

public class WeatherForecastHub : Hub<IWeatherForecastHubClient>
{
    private readonly ISender _sender;
    public WeatherForecastHub(ISender sender) =>
        _sender = sender;

    public override async Task OnConnectedAsync()
    {
        var message = await _sender.Send(new WeatherForcastsClientQuery(Context.ConnectionId));
        await Clients.All.ReceiveMessage("System", message);
        await base.OnConnectedAsync();
    }
}
