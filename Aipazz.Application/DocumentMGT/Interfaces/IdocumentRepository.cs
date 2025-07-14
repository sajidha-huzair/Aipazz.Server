using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.DocumentMgt;

namespace Aipazz.Application.DocumentMGT.Interfaces
{
    public interface IdocumentRepository
    {
        Task<List<Document>> GetAllDocuments();
        Task SaveAsync(Document document);

        Task<Document?> GetByIdAsync(string documentId, string userId);
        Task UpdateAsync(Document document);

        Task<List<Document>> GetAllByUserIdAsync(string userId);
        Task DeleteAsync(string documentId, string userId);

        Task<List<Document>> GetDocumentsByTeamIdsAsync(List<string> teamIds);
    }
}
