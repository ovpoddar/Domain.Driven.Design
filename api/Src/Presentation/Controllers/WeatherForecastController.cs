using DDD.Application.WeatherForecasts.GetWeatherForecasts.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Presentation.Controllers;

[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ISender _sender;

    public WeatherForecastController(ISender sender) =>
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetWeatherForecast(string name)
    {
        var result = await _sender.Send(new GetWeatherForecastsQuery(name));
        return Ok(result);
    }

}
