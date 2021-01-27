using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using RoosterPlanner.Service.Config;

namespace RoosterPlanner.Service
{
    public interface IBlobService
    {
        /// <summary>
        /// Uploades a file to blobstorage.
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="blobName"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Task<Uri> UploadFileBlobAsync(string blobContainerName, string blobName, Stream content, string contentType);

        /// <summary>
        /// deletes a file from blobstorage.
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="blobName"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Task<bool> DeleteFileBlobAsync(string blobContainerName, string blobName);
    }

    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(IOptions<AzureBlobConfig> azureBlobConfig)
        {
            blobServiceClient = new BlobServiceClient(azureBlobConfig.Value.AzureBlobConnectionstring);
        }

        /// <summary>
        /// Uploades a file to blobstorage.
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="blobName"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<Uri> UploadFileBlobAsync(string blobContainerName, string blobName, Stream content,
            string contentType)
        {
            BlobContainerClient containerClient = GetContainerClient(blobContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, new BlobHttpHeaders {ContentType = contentType});
            return blobClient.Uri;
        }

        /// <summary>
        /// deletes a file from blobstorage.
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="blobName"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFileBlobAsync(string blobContainerName, string blobName)
        {
            BlobContainerClient containerClient = GetContainerClient(blobContainerName);
            BlobClient blob = containerClient.GetBlobClient(blobName);
            return await blob.DeleteIfExistsAsync();
        }
        /// <summary>
        /// Gets the blobcontainerclient from a containername. 
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <returns></returns>
        private BlobContainerClient GetContainerClient(string blobContainerName)
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);
            return containerClient;
        }
    }
}