namespace DDD.Domain.Entities.WeatherForecastHistory;

public class WeatherForecast
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public DateOnly Date { get; }
    public int TemperatureC { get; }
    public string? Summary { get; }

    public WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        this.Date = Date;
        this.TemperatureC = TemperatureC;
        this.Summary = Summary;
    }
}
