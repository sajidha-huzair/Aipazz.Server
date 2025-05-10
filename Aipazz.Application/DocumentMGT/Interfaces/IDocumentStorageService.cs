using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.DocumentMGT.Interfaces
{
    public interface IDocumentStorageService
    {
        Task<string> SaveDocumentAsync(string fileName, string contentHtml);


    }
}
