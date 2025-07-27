using Microsoft.AspNetCore.Http;

namespace Aipazz.Application.OtherDocuments.Interfaces
{
    public interface IOtherDocumentStorageService
    {
        Task<string> SaveFileAsync(string userId, string documentId, string fileName, IFormFile file);
        Task<bool> DeleteFileAsync(string fileUrl);
        Task<(byte[] content, string contentType, string fileName)> GetFileAsync(string fileUrl);
    }
}