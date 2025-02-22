using FluentValidation;
using System.Reflection.Metadata.Ecma335;

namespace DDD.Application.WeatherForecasts.GetWeatherForecasts.Query;

public sealed class GetWeatherForecastValidator : AbstractValidator<GetWeatherForecastsQuery>
{
    public GetWeatherForecastValidator()
    {
        RuleFor(a => a.name)
            .NotEmpty();
    }
}

public sealed class GetWeatherForecastValidator2 : AbstractValidator<GetWeatherForecastsQuery>
{
    public GetWeatherForecastValidator2()
    {
        RuleFor(a => a.name)
            .MustAsync((a, b) =>
            {
                return Task.FromResult(false);
            })
            .NotEmpty();
    }
}
