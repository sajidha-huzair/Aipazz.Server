using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;

namespace AIpazz.Infrastructure.Documentmgt
{
    public class TemplateRepository : ITemplateRepository
    {
        public Task<List<Template>> GetAllTemplates()
        {
           var templates =  new List<Template>
            {
                new Template
                {
                    id = Guid.NewGuid().ToString(),
                    Name = "Sample Template 1",
                    Category = "Category1",
                    Url = "/doc.pdf"
                },
                new Template
                {
                    id = Guid.NewGuid().ToString(),
                    Name = "Sample Template 2",
                    Category = "Category2",
                    Url = "/doc.pdf"
                }
            };
            return Task.FromResult(templates);
        }
    }
}
