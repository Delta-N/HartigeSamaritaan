using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Service;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IBlobService blobService;
        private readonly ILogger logger;

        public UploadController(IBlobService blobService, ILogger<UploadController> logger)
        {
            this.blobService = blobService;
            this.logger = logger;
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPost, RequestSizeLimit(100_000_00)]
        public async Task<ActionResult> UploadInstruction(string containerName = "instructiondocuments")
        {
            try
            {
                if (Request.Form.Files.FirstOrDefault()==null)
                    return BadRequest("No file received");
                IFormFile file = Request.Form.Files[0];
                string extension = Path.GetExtension(file.FileName);

                Uri result = await blobService.UploadFileBlobAsync(
                    containerName,
                    Guid.NewGuid() + extension,
                    file.OpenReadStream(),
                    file.ContentType
                    );

                return Ok(new UploadResultViewModel()
                {
                    Path = result.AbsoluteUri,
                    Succeeded = true
                });
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity(new UploadResultViewModel(){Succeeded = false});
            }
        }

        [HttpDelete()]
        public async Task<ActionResult> Delete(string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest("No valid Url received");

            try
            {
                Uri uri = new Uri(url);
                string blobfilename = Path.GetFileName(uri.LocalPath);
                string blobContainerName = uri.AbsolutePath.Substring(1, uri.AbsolutePath.IndexOf('/', 1) - 1);
                bool result = await blobService.DeleteFileBlobAsync(blobContainerName, blobfilename);
                return Ok(new UploadResultViewModel(){Succeeded = result});
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity(new UploadResultViewModel(){Succeeded = false});
            }
        }

        [HttpPost("UploadProfilePicture"), RequestSizeLimit(500_000_0)]
        public async Task<ActionResult> UploadProfilePicture()
        {
            return await UploadInstruction("profilepicture");
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPost("UploadProjectPicture"), RequestSizeLimit(500_000_0)]
        public async Task<ActionResult> UploadProjectPicture()
        {
            return await UploadInstruction("projectpicture");
        }
    }
}