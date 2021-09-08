using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace WebApi.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string storageAccountConnectionStr;
        private readonly ILogger logger;

        public BlobStorageService(ILogger logger)
        {
            this.logger = logger;
            storageAccountConnectionStr = Environment.GetEnvironmentVariable("storageAccountConnectionStr");
        }

        public IEnumerator<BlobItem> GetFilesFromDataContainer()
        {
            var blobServiceClient = new BlobServiceClient(storageAccountConnectionStr);
            var blobContainer = blobServiceClient.GetBlobContainerClient("data");

            return blobContainer
                .GetBlobs()
                .GetEnumerator();
        }

        public string GetFileDataByFileName(string fileName)
        {
            var blobServiceClient = new BlobServiceClient(storageAccountConnectionStr);
            var blobContainer = blobServiceClient.GetBlobContainerClient("data");
            var blobClient = blobContainer.GetBlobClient(fileName);

            var file = blobClient.DownloadContent();
            return file.Value.Content.ToString();
        }
    }
}
