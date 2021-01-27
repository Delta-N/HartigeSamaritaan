using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IDocumentService
    {
        /// <summary>
        /// Makes a call to the repository layer and requests a document based on a personId and a id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<TaskResult<Document>> GetDocumentAsync(Guid documentId);

        /// <summary>
        /// Makes a call to the repository layer and adds a document to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task<TaskResult<Document>> CreateDocumentAsync(Document document);

        /// <summary>
        /// Makes a call to the repository layer and requests a update of a document.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task<TaskResult<Document>> UpdateDocumentAsync(Document document);

        /// <summary>
        /// Makes a call to the repository layer and requests a deletion of a document.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task<TaskResult<Document>> DeleteDocumentAsync(Document document);

        /// <summary>
        /// Makes a call to the repository layer and requests the privacy policy.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <returns></returns>
        Task<TaskResult<Document>> GetPPAsync();
    }

    public class DocumentService : IDocumentService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IDocumentRepository documentRepository;
        private readonly ILogger<DocumentService> logger;
        private readonly IBlobService blobService;

        #endregion

        public DocumentService(
            IUnitOfWork unitOfWork,
            ILogger<DocumentService> logger,
            IBlobService blobService)

        {
            this.unitOfWork = unitOfWork;
            documentRepository = unitOfWork.DocumentRepository;
            this.logger = logger;
            this.blobService = blobService;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a document based on a personId and a id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public async Task<TaskResult<Document>> GetDocumentAsync(Guid documentId)
        {
            if (documentId == Guid.Empty)
                return null;

            TaskResult<Document> result = new TaskResult<Document>();

            try
            {
                result.Data = await documentRepository.GetAsync(documentId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting document " + documentId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and adds a document to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async Task<TaskResult<Document>> CreateDocumentAsync(Document document)
        {
            if (document == null)
                return null;

            TaskResult<Document> result = new TaskResult<Document>();

            try
            {
                result.Data = documentRepository.Add(document);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating document " + document.Id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a update of a document.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async Task<TaskResult<Document>> UpdateDocumentAsync(Document document)
        {
            if (document == null)
                return null;

            TaskResult<Document> result = new TaskResult<Document>();

            try
            {
                result.Data = documentRepository.Update(document);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating document " + document.Id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a deletion of a document.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async Task<TaskResult<Document>> DeleteDocumentAsync(Document document)
        {
            if (document == null)
                return null;

            TaskResult<Document> result = new TaskResult<Document>();

            try
            {
                result.Data = documentRepository.Remove(document);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
                Uri uri = new Uri(document.DocumentUri);
                string blobfilename = Path.GetFileName(uri.LocalPath);
                string blobContainerName = uri.AbsolutePath.Substring(1, uri.AbsolutePath.IndexOf('/', 1) - 1);
                await blobService.DeleteFileBlobAsync(blobContainerName, blobfilename);
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error deleting document " + document.Id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests the privacy policy.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <returns></returns>
        public async Task<TaskResult<Document>> GetPPAsync()
        {
            TaskResult<Document> result = new TaskResult<Document>();
            try
            {
                result.Data = await documentRepository.GetPPAsync();
                if (result.Data != null)
                    result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting Privacy Policy ";
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }
    }
}