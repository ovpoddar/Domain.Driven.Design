using DDD.Domain.Entities.WeatherForecastHistory;
using MediatR;

namespace DDD.Application.WeatherForecasts.GetWeatherForecasts.Query;

public record GetWeatherForecastsQuery(string name) : IRequest<IEnumerable<WeatherForecast>>;