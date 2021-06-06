using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobQuickstartV12 : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            // See here https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
            // Creating Blob Container
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            //Create a unique name for the container
            string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            return Ok(containerClient);
        }
    }
}
