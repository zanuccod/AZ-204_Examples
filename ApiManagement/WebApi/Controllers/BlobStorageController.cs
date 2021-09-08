using System;
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

        public BlobStorageController(IBlobStorageService blobStorageService, ILogger logger)
        {
            this.blobStorageService = blobStorageService;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetBlobList()
        {
            var blobs = blobStorageService.GetFilesFromDataContainer();
            return Ok(blobs);
        }

        [HttpGet]
        public IActionResult GetDeleteObjFile()
        {
            var deleteObjFile = blobStorageService.GetFileDataByFileName("DeleteBinObj.sh");
            return Ok(deleteObjFile);
        }
    }
}
