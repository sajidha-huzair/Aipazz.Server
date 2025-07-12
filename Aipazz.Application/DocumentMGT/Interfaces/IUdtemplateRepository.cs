using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.DocumentMgt;

namespace Aipazz.Application.DocumentMGT.Interfaces
{
    public interface IUdtemplateRepository
    {
        Task<List<Udtemplate>> GetAllTemplates();
        Task<List<Udtemplate>> GetAllTemplatesByUserId(string userId);
        Task<Udtemplate?> GetTemplateById(string id);
        Task<Udtemplate?> GetTemplateByIdAndUserId(string id, string userId);
        Task CreateTemplate(Udtemplate Udtemplate);
        Task UpdateTemplate(Udtemplate Udtemplate);
        Task DeleteTemplate(string id);
        Task DeleteTemplate(string id, string userId);
    }
}
