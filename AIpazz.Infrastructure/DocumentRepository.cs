using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application;
using Aipazz.Domian;

namespace AIpazz.Infrastructure
{
    public class DocumentRepository : IDocumentRepository
    {
        List<Document> _documents = new List<Document> {
        new Document { ID = "1", Title = "Document 1", Url = "http://example.com/doc1" },
        new Document { ID = "2", Title = "Document 2", Url = "http://example.com/doc2" },
        new Document { ID = "3", Title = "Document 3", Url = "http://example.com/doc3" }
        };
        public List<Document> GetAllDocuments()
        {
            return _documents;
        }
    }
}
