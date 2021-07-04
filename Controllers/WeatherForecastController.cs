using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFirstAzureWebApp.Business;
using MyFirstAzureWebApp.Models;
using System.Collections.Generic;

namespace MyFirstAzureWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        

        private readonly ILogger<WeatherForecastController> _logger;
        private Weather weather;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {

            _logger = logger;
            weather = new Weather();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Information : Hello, this is the WeatherForecast!");
            _logger.LogTrace("Trace : Hello, this is the WeatherForecast!");
            _logger.LogDebug("Debug : Hello, this is the WeatherForecast!");
            _logger.LogError("Error : Hello, this is the WeatherForecast!");
            _logger.LogWarning("Error : Hello, this is the WeatherForecast!");
            return weather.Forecast();
        }

        [HttpGet("{id}")]
        public IEnumerable<WeatherForecast> GetById(string id)
        {
            return weather.ForecastById(id);            
        }
    }
}
