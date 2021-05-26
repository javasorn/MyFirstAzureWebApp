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
            return weather.Forecast();
        }

        [HttpGet("{id}")]
        public IEnumerable<WeatherForecast> GetById(string id)
        {
            return weather.ForecastById(id);            
        }
    }
}
