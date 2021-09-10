using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace WebApi.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string storageAccountConnectionStr;
        private readonly ILogger logger;

        public BlobStorageService(ILogger<BlobStorageService> logger)
        {
            this.logger = logger;
            storageAccountConnectionStr = Environment.GetEnvironmentVariable("storageAccountConnectionStr");
        }

        public async Task<IEnumerable<string>> GetFilesFromDataContainerAsync()
        {
            var blobServiceClient = new BlobServiceClient(storageAccountConnectionStr);
            var blobContainer = blobServiceClient.GetBlobContainerClient("data");

            var items = blobContainer
                .GetBlobsAsync()
                .GetAsyncEnumerator();

            var tmp = new List<string>();
            while (await items.MoveNextAsync())
            {
                tmp.Add(items.Current.Name);
            }
            return tmp;
        }

        public async Task<string> GetFileDataByFileNameAsync(string fileName)
        {
            var blobServiceClient = new BlobServiceClient(storageAccountConnectionStr);
            var blobContainer = blobServiceClient.GetBlobContainerClient("data");
            var blobClient = blobContainer.GetBlobClient(fileName);

            var file = await blobClient.DownloadContentAsync();
            return file.Value.Content.ToString();
        }
    }
}
