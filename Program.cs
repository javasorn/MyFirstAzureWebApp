using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    var settings = config.Build();
                    if (!context.HostingEnvironment.IsDevelopment())
                    {
                        var keyVaultEndpoint = settings["AzureKeyVaultEndpoint"];
                        if (!string.IsNullOrEmpty(keyVaultEndpoint))
                        {
                            var azureServiceTokenProvider = new AzureServiceTokenProvider();
                            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                            config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                        }
                        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", settings["dev-uat-database-saleonmob"]);
                        Environment.SetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING", settings["dev-uat-storage-saleonmob"]);
                    }
                    else
                    {
                        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", settings["DatabaseConnectionString"]);
                        Environment.SetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING", settings["BlobConnectionString"]);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
