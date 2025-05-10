using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.DocumentMGT.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveWordDocumentAsync(string userId, string fileName, byte[] content);
        Task<string> SaveHtmlContentAsync(string userId, string fileName, string htmlContent);
    }
}
