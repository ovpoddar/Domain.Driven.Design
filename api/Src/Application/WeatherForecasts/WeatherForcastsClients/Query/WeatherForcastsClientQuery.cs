using DDD.Domain.Entities.WeatherForecastHistory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.WeatherForecasts.WeatherForcastsClients.Query;

public record WeatherForcastsClientQuery(string UserId) : IRequest<string>;
