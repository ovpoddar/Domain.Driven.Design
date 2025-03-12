using DDD.Application.WeatherForecasts.GetWeatherForecasts.Query;
using DDD.Application.WeatherForecasts.WeatherForcastsClients.Query;
using DDD.Domain.Entities.WeatherForecastHistory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.WeatherForecasts.WeatherForcastsClients.Handler;

public class WeatherForcastsClientHandler : IRequestHandler<WeatherForcastsClientQuery, string>
{
    public async Task<string> Handle(WeatherForcastsClientQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return $"{request.UserId} has joined. for latest weather update.";
    }
}
