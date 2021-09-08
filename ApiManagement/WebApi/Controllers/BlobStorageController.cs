using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IBlobStorageService blobStorageService;
        private readonly ILogger logger;

        public BlobStorageController(IBlobStorageService blobStorageService, ILogger<BlobStorageController> logger)
        {
            this.blobStorageService = blobStorageService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("get-blob-list")]
        public async Task<IActionResult> GetBlobListAsync()
        {
            var blobs = await blobStorageService.GetFilesFromDataContainerAsync();
            return Ok(new
            {
                Count = blobs.Count(),
                Items = blobs
            });
        }

        [HttpGet]
        [Route("get-delete-file")]
        public async Task<IActionResult> GetDeleteObjFileAsync()
        {
            var deleteObjFile = await blobStorageService.GetFileDataByFileNameAsync("DeleteBinObj.sh");
            return Ok(deleteObjFile);
        }
    }
}
