using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RoosterPlanner.Service
{
    public interface IBlobService
    {
        Task<Uri> UploadFileBlobAsync(string blobContainerName, Stream content, string contentType, string fileName);
        Task<bool> DeleteFileBlobAsync(string blobContainerName, string blobName);
    }
    

    public class BlobService :IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<Uri> UploadFileBlobAsync(string blobContainerName, Stream content, string contentType, string fileName)
        {
            BlobContainerClient containerClient = GetContainerClient(blobContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });
            return blobClient.Uri;
        }

        public async Task<bool> DeleteFileBlobAsync(string blobContainerName, string blobName)
        {
            BlobContainerClient containerClient = GetContainerClient(blobContainerName);
            BlobClient blob = containerClient.GetBlobClient(blobName);
            return await blob.DeleteIfExistsAsync();
        }

        private BlobContainerClient GetContainerClient(string blobContainerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);
            return containerClient;
        }
    }
}