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
                Language = _configuration["Settings-Language"],

                ConnectionString = _configuration[key]
            };

            return Ok(settings);
        }
        [HttpGet]
        [Route("get-key")]
        public IActionResult GetKeys()
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
            {
                Delay= TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
            };

            var keyVaultEndpoint = "https://dev-uat-it-keyvault.vault.azure.net";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var client = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret("dev-uat-database-saleonmob");

            string secretValue = secret.Value;

            return Ok(secret);
        }
        //.ConfigureAppConfiguration((context, config) =>
        //{
        //    var settings = config.Build();
        //    if (!context.HostingEnvironment.IsDevelopment())
        //    {
        //        var keyVaultEndpoint = settings["AzureKeyVaultEndpoint"];
        //        if (!string.IsNullOrEmpty(keyVaultEndpoint))
        //        {
        //            var azureServiceTokenProvider = new AzureServiceTokenProvider();
        //            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        //            config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        //        }
        //    }
        //})
    }
}
