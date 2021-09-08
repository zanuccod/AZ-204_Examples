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
            try
            {
                logger.LogInformation("GetBlobListAsync start");

                var blobs = await blobStorageService.GetFilesFromDataContainerAsync();

                logger.LogInformation("blobStorageService.GetFilesFromDataContainerAsync completed");

                return Ok(new
                {
                    Count = blobs.Count(),
                    Items = blobs
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process end-point <{functionName}>", System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name);
                return new ObjectResult($"Failed to process end-point <{System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name}>: {ex.Message}") { StatusCode = 500 };
            }
        }

        [HttpGet]
        [Route("get-delete-file")]
        public async Task<IActionResult> GetDeleteObjFileAsync()
        {
            try
            {
                logger.LogInformation("GetDeleteObjFileAsync start");

                var deleteObjFile = await blobStorageService.GetFileDataByFileNameAsync("DeleteBinObj.sh");
                logger.LogInformation("blobStorageService.GetFileDataByFileNameAsync completed");

                return Ok(deleteObjFile);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process end-point <{functionName}>", System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name);
                return new ObjectResult($"Failed to process end-point <{System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name}>: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
