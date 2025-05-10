using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace AIpazz.Infrastructure.Documentmgt.Services
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "documents";

        public AzureBlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> SaveWordDocumentAsync(string userId, string documentId,  string fileName, byte[] content)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            var blobName = $"{userId}/{documentId}_{fileName}.docx";
            var blobClient = blobContainer.GetBlobClient(blobName);

            using var stream = new MemoryStream(content);
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<string> SaveHtmlContentAsync(string userId, string documentId, string fileName, string htmlContent)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            var blobName = $"{userId}/{documentId}_{fileName}.html";
            var blobClient = blobContainer.GetBlobClient(blobName);

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent));
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<string> UpdateWordDocumentAsync(string userId, string documentId, string fileName, byte[] content)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            var blobName = $"{userId}/{documentId}_{fileName}.docx";
            var blobClient = blobContainer.GetBlobClient(blobName);

            using var stream = new MemoryStream(content);
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<string> UpdateHtmlContentAsync(string userId, string documentId, string fileName, string htmlContent)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            var blobName = $"{userId}/{documentId}_{fileName}.html";
            var blobClient = blobContainer.GetBlobClient(blobName);

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent));
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

    }
}
