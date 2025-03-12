using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Abstractions.Hub;

public interface IWeatherForecastHubClient
{
    Task ReceiveMessage(string senderId, string message);
}
