using Aipazz.Domian.OtherDocuments;

namespace Aipazz.Application.OtherDocuments.Interfaces
{
    public interface IOtherDocumentRepository
    {
        Task<string> SaveAsync(OtherDocument document);
        Task<OtherDocument?> GetByIdAsync(string id, string userId);
        Task<IEnumerable<OtherDocument>> GetAllByUserIdAsync(string userId);
        Task<IEnumerable<OtherDocument>> GetByMatterIdAsync(string matterId, string userId);
        Task<bool> DeleteAsync(string id, string userId);
        Task<bool> UpdateAsync(OtherDocument document);
        Task ShareToTeamAsync(string documentId, string teamId, string userId);
        Task<bool> RemoveFromTeamAsync(string documentId, string userId);
        Task<IEnumerable<OtherDocument>> GetTeamSharedDocumentsAsync(string userId);
    }
}