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
        Task<TaskResult<Document>> GetDocumentAsync(Guid documentId);
        Task<TaskResult<Document>> CreateDocumentAsync(Document document);
        Task<TaskResult<Document>> UpdateDocumentAsync(Document document);
        Task<TaskResult<Document>> DeleteDocumentAsync(Document document);
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
            this.documentRepository = unitOfWork.DocumentRepository;
            this.logger = logger;
            this.blobService = blobService;
        }

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