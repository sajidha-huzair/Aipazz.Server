using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian;

namespace Aipazz.Application
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        // Constructor injection of the repository
        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public List<Document> GetAllDocuments()
        {
            return _documentRepository.GetAllDocuments();
        }
    }
}
