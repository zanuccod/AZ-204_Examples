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
        private const string storageAccountConnectionStr = "DefaultEndpointsProtocol=https;AccountName=apimstoragetest;AccountKey=Fy6cRfdEt09GtBe7ylapN98WH5bCUZ088okjOfLICWDpwRcs0PGH5pNAp3xViVUjNIW4mP1YFShbJRrPSRZAGg==;EndpointSuffix=core.windows.net";
        private readonly ILogger logger;

        public BlobStorageService(ILogger logger)
        {
            this.logger = logger;
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
