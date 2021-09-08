using System;
using System.Collections.Generic;
using Azure.Storage.Blobs.Models;

namespace WebApi.Services
{
    public interface IBlobStorageService
    {
        IEnumerator<BlobItem> GetFilesFromDataContainer();
        string GetFileDataByFileName(string fileName);
    }
}
