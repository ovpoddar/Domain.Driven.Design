using DDD.Domain.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Entities.LoggingEntity;

public class Logging : Entity<int>
{
    public string Message { get; set; }
    public string MessageTemplate { get; set; }
    public string Level { get; set; }
    public string Exception { get; set; }
    public string Properties { get; set; }
    public string LogEvent { get; set; }

    public Logging(int id, string message, string messageTemplate, string level, string exception, string properties, string logEvent) : base(id)
    {
        Message = message;
        MessageTemplate = messageTemplate;
        Level = level;
        Exception = exception;
        Properties = properties;
        LogEvent = logEvent;
    }
}
