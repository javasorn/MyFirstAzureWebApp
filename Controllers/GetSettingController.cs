using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using MyFirstAzureWebApp.Models;
using System;

namespace MyFirstAzureWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetSettingController : Controller
    {
        private readonly IConfiguration _configuration;
        public GetSettingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("get-settings")]
        public IActionResult GetSettings(string key)
        {
            var settings = new Settings
            {
                AppName = _configuration["Settings-AppName"],
                ConnectionString = _configuration[key]
            };

            return Ok(settings);
        }
        
    }
}
