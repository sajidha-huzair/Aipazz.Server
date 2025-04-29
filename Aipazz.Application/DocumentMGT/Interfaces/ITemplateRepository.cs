using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.DocumentMgt;

namespace Aipazz.Application.DocumentMGT.Interfaces
{
    public interface ITemplateRepository
    {
        Task<List<Template>> GetAllTemplates();
        Task<Template?> GetTemplateById(string id);
        Task CreateTemplate(Template template);
        Task UpdateTemplate(Template template);
        Task DeleteTemplate(string id);
    }
}
