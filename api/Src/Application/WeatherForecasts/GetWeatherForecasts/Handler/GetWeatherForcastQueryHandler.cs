using DDD.Application.Abstractions.Hub;
using DDD.Application.WeatherForecasts.GetWeatherForecasts.Query;
using DDD.Domain.Entities.WeatherForecastHistory;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace DDD.Application.WeatherForecasts.GetWeatherForecasts.Handler;

public class GetWeatherForecastsQueryHandler : IRequestHandler<GetWeatherForecastsQuery, IEnumerable<WeatherForecast>>
{
    private static readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
    private readonly IHubConnection<IWeatherForecastHubClient> _hubConnection;

    public GetWeatherForecastsQueryHandler(IHubConnection<IWeatherForecastHubClient> hubConnection) =>
        _hubConnection = hubConnection;

    public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
    {
        var result = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
            ));
        await _hubConnection.Clients.All.ReceiveMessage("user", JsonSerializer.Serialize(result));
        return result;
    }
}

