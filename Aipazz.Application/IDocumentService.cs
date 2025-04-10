using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian;

namespace Aipazz.Application
{
    public interface IDocumentService
    {
        List<Document> GetAllDocuments();
    }
}
