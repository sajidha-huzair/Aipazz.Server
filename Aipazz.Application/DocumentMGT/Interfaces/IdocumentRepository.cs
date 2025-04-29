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

    }
}
