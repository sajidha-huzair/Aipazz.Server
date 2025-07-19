using Aipazz.Application.OtherDocuments.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace AIpazz.Infrastructure.OtherDocuments.Services
{
    public class OtherDocumentStorageService : IOtherDocumentStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "otherdocuments";

        public OtherDocumentStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> SaveFileAsync(string userId, string documentId, string fileName, IFormFile file)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            var blobName = $"{userId}/{fileName}";
            var blobClient = blobContainer.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
                
                var blobName = GetBlobNameFromUrl(fileUrl);
                var blobClient = blobContainer.GetBlobClient(blobName);

                return await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<(byte[] content, string contentType, string fileName)> GetFileAsync(string fileUrl)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            
            var blobName = GetBlobNameFromUrl(fileUrl);
            var blobClient = blobContainer.GetBlobClient(blobName);

            var response = await blobClient.DownloadContentAsync();
            var properties = await blobClient.GetPropertiesAsync();

            var fileName = Path.GetFileName(blobName);
            return (response.Value.Content.ToArray(), properties.Value.ContentType, fileName);
        }

        private string GetBlobNameFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return string.Join('/', segments.Skip(1)); // Skip container name
        }
    }
}