using FluentValidation;

namespace DDD.Application.WeatherForecasts.GetWeatherForecasts.Query;

public sealed class GetWeatherForecastValidator : AbstractValidator<GetWeatherForecastsQuery>
{
    public GetWeatherForecastValidator()
    {
        RuleFor(a => a.name)
            .MustAsync((a, b) =>
            {
                return Task.FromResult(false);
            })
            .NotEmpty();
    }
}
