using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Configuration;

public static class ConfigurationHelper
{
    private const string _configurationSectionName = "DefaultConfiguration";

    public static T? GetConfiguration<T>(this IConfiguration configuration, string name) => 
        configuration.GetSection($"{_configurationSectionName}:{name}")
        .Get<T>();

    public static IConfigurationSection GetConfiguration(this IConfiguration configuration, string name) =>
        configuration.GetSection($"{_configurationSectionName}:{name}");
}
