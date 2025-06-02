using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.DocumentMGT.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveWordDocumentAsync(string userId, string documentId, string fileName, byte[] content);
        Task<string> SaveHtmlContentAsync(string userId, string documentId, string fileName, string htmlContent);
        Task<string> UpdateWordDocumentAsync(string userId, string documentId, string fileName, byte[] content);
        Task<string> UpdateHtmlContentAsync(string userId, string documentId, string fileName, string htmlContent);

        Task<string> DeleteDocumentAsync(string userId, string documentId, string fileName);

    }
}
