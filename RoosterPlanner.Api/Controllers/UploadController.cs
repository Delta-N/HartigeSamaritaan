using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models.EntityViewModels;
using RoosterPlanner.Api.Models.HelperViewModels;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Models.Models.Enums;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using RoosterPlanner.Service.Services;
using Type = RoosterPlanner.Api.Models.HelperViewModels.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IBlobService blobService;
        private readonly IDocumentService documentService;
        private readonly ILogger<UploadController> logger;

        public UploadController(
            IBlobService blobService,
            ILogger<UploadController> logger,
            IDocumentService documentService)
        {
            this.blobService = blobService;
            this.logger = logger;
            this.documentService = documentService;
        }

        /// <summary>
        /// Upload a instruction pdf to Blobstorage.
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPost, RequestSizeLimit(100_000_00)]
        public async Task<ActionResult<UploadResultViewModel>> UploadInstructionAsync(
            string containerName = "instructiondocuments")
        {
            try
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                if (file == null)
                    return BadRequest("No file received");

                string extension = Path.GetExtension(file.FileName);

                Uri result = await blobService.UploadFileBlobAsync(
                    containerName,
                    Guid.NewGuid() + extension,
                    file.OpenReadStream(),
                    file.ContentType
                );

                return Ok(new UploadResultViewModel
                {
                    Path = result.AbsoluteUri,
                    Succeeded = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, GetType().Name + "Error in " + nameof(UploadInstructionAsync));
                return UnprocessableEntity(new UploadResultViewModel {Succeeded = false});
            }
        }

        /// <summary>
        /// Delete data from blobstorage
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<UploadResultViewModel>> DeleteAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest("No valid Url received");

            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = "UrI not correctly formatted"});

                Uri uri = new Uri(url);
                string blobfilename = Path.GetFileName(uri.LocalPath);
                string blobContainerName = uri.AbsolutePath.Substring(1, uri.AbsolutePath.IndexOf('/', 1) - 1);
                bool result = await blobService.DeleteFileBlobAsync(blobContainerName, blobfilename);
                return Ok(new UploadResultViewModel {Succeeded = result});
            }
            catch (Exception ex)
            {
                logger.LogError(ex, GetType().Name + "Error in " + nameof(DeleteAsync));
                return UnprocessableEntity(new UploadResultViewModel {Succeeded = false});
            }
        }

        /// <summary>
        /// Upload a profilepicture to blobstorage
        /// </summary>
        /// <returns></returns>
        [HttpPost("UploadProfilePicture"), RequestSizeLimit(500_000_0)]
        public async Task<ActionResult<UploadResultViewModel>> UploadProfilePictureAsync()
        {
            return await UploadInstructionAsync("profilepicture");
        }

        /// <summary>
        /// Upload a projectpicture
        /// Only a Boardmember can upload a projectpicture 
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPost("UploadProjectPicture"), RequestSizeLimit(500_000_0)]
        public async Task<ActionResult<UploadResultViewModel>> UploadProjectPictureAsync()
        {
            return await UploadInstructionAsync("projectpicture");
        }

        /// <summary>
        /// Upload a privacy policy
        /// Only a Boardmember can upload a privacy policy 
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPost("UploadPP"), RequestSizeLimit(100_000_00)]
        public async Task<ActionResult<UploadResultViewModel>> UploadPP()
        {
            return await UploadInstructionAsync("privacypolicy");
        }

        /// <summary>
        /// Makes a request towards the services layer to add a document in the database.
        /// </summary>
        /// <param name="documentViewModel"></param>
        /// <returns></returns>
        [HttpPost("document")]
        public async Task<ActionResult<DocumentViewModel>> CreateDocument(DocumentViewModel documentViewModel)
        {
            if (documentViewModel?.Name == null || documentViewModel.DocumentUri == null)
                return BadRequest("No valid document received");
            try
            {
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                if (documentViewModel.Name == "Privacy Policy" && !PersonsController.UserHasRole(UserRole.Boardmember,
                    (ClaimsIdentity) HttpContext.User.Identity))
                    return Unauthorized();

                Document document = DocumentViewModel.CreateDocument(documentViewModel);
                if (document == null)
                    return BadRequest("Unable to convert DocumentViewModel to Document");

                document.LastEditBy = oid;
                TaskResult<Document> result;
                if (document.Id == Guid.Empty)
                    result = await documentService.CreateDocumentAsync(document);
                else
                    return BadRequest("Cannot update existing document with post method");
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(DocumentViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(CreateDocument);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to update a document.
        /// </summary>
        /// <param name="documentViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<DocumentViewModel>> UpdateDocument(DocumentViewModel documentViewModel)
        {
            if (documentViewModel?.Name == null || documentViewModel.DocumentUri == null ||
                documentViewModel.Id == Guid.Empty)
                return BadRequest("No valid document received");
            try
            {
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                if (documentViewModel.Name == "TOS" && !PersonsController.UserHasRole(UserRole.Boardmember,
                    (ClaimsIdentity) HttpContext.User.Identity))
                    return Unauthorized();

                Document updatedDocument = DocumentViewModel.CreateDocument(documentViewModel);
                if (updatedDocument == null)
                    return BadRequest("Unable to convert DocumentViewModel to Document");

                Document oldDocument = (await documentService.GetDocumentAsync(updatedDocument.Id)).Data;
                if (oldDocument == null)
                    return NotFound("Document not found");
                if (!oldDocument.RowVersion.SequenceEqual(documentViewModel.RowVersion))
                    return BadRequest("Outdated entity received");

                oldDocument.Name = updatedDocument.Name;
                oldDocument.DocumentUri = updatedDocument.DocumentUri;

                oldDocument.LastEditBy = oid;

                TaskResult<Document> result = await documentService.UpdateDocumentAsync(oldDocument);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(DocumentViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(CreateDocument);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer for the privacy policy document entity.
        /// </summary>
        /// <returns></returns>
        [HttpGet("PrivacyPolicy")]
        public async Task<ActionResult<DocumentViewModel>> GetPP()
        {
            try
            {
                TaskResult<Document> PP = await documentService.GetPPAsync();
                if (!PP.Succeeded)
                    return NotFound("Privacy Policy not found");
                return Ok(DocumentViewModel.CreateVm(PP.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(CreateDocument);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to delete a document.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("document/{id}")]
        public async Task<ActionResult<DocumentViewModel>> DeleteDocumentAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                Document document = (await documentService.GetDocumentAsync(id)).Data;
                if (document == null)
                    return NotFound("Document not found");

                if (document.Name != "profilepicture" && !PersonsController.UserHasRole(UserRole.Boardmember,
                    (ClaimsIdentity) HttpContext.User.Identity))
                    return Unauthorized("User is cannot delete this file");

                TaskResult<Document> removeDocumentResult = await documentService.DeleteDocumentAsync(document);
                return !removeDocumentResult.Succeeded
                    ? UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = removeDocumentResult.Message})
                    : Ok(DocumentViewModel.CreateVm(removeDocumentResult.Data));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, GetType().Name + "Error in " + nameof(DeleteAsync));
                return UnprocessableEntity(new UploadResultViewModel {Succeeded = false});
            }
        }
    }
}