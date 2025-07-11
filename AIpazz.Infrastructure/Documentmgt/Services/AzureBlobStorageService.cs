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
        public readonly string _templateContainerName = "templates";

        public AzureBlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> SaveWordDocumentAsync(string userId, string documentId, string fileName, byte[] content)
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

        public async Task<string> DeleteDocumentAsync(string wordUrl, string htmlUrl)
        {
            Console.WriteLine(htmlUrl);
            Console.WriteLine(wordUrl);
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            string GetBlobNameFromUrl(string url)
            {
                var uri = new Uri(url);
                var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                return string.Join('/', segments.Skip(1)); // Skip container name
            }

            var wordBlobName = GetBlobNameFromUrl(wordUrl);
            var htmlBlobName = GetBlobNameFromUrl(htmlUrl);

            var wordBlobClient = blobContainer.GetBlobClient(wordBlobName);
            var htmlBlobClient = blobContainer.GetBlobClient(htmlBlobName);

            bool wordDeleted = await wordBlobClient.DeleteIfExistsAsync();
            bool htmlDeleted = await htmlBlobClient.DeleteIfExistsAsync();

            if (wordDeleted && htmlDeleted)
            {
                return "Both Word and HTML documents were successfully deleted.";
            }
            else if (wordDeleted && !htmlDeleted)
            {
                return "Word document deleted. HTML document not found.";
            }
            else if (!wordDeleted && htmlDeleted)
            {
                return "HTML document deleted. Word document not found.";
            }
            else
            {
                return "No documents found to delete.";
            }
        }

        public async Task<string> SaveTemplateAsync(string id, string templateName, string contentHtml)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_templateContainerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            var blobName = $"{id}_{templateName}.html";
            var blobClient = blobContainer.GetBlobClient(blobName);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(contentHtml));
            await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteTemplateAsync(string templateUrl)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_templateContainerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            string GetBlobNameFromUrl(string url)
            {
                var uri = new Uri(url);
                var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                return string.Join('/', segments.Skip(1)); // Skip container name
            }

            var blobName = GetBlobNameFromUrl(templateUrl);
            var blobClient = blobContainer.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
