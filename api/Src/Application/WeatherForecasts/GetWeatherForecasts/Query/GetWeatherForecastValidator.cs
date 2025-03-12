using FluentValidation;
using System.Reflection.Metadata.Ecma335;

namespace DDD.Application.WeatherForecasts.GetWeatherForecasts.Query;

public sealed class GetWeatherForecastValidator : AbstractValidator<GetWeatherForecastsQuery>
{
    public GetWeatherForecastValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty();
    }
}

public sealed class GetWeatherForecastValidator2 : AbstractValidator<GetWeatherForecastsQuery>
{
    public GetWeatherForecastValidator2()
    {
        RuleFor(a => a.Name)
            .MustAsync((a, b) =>
            {
                return Task.FromResult(a.Length > 2);
            });
    }
}
