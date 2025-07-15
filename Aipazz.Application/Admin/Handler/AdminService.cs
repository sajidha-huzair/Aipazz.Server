using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Admin.Interface;

namespace Aipazz.Application.Admin.Handler
{
    public class AdminService : IAdminService
    {
        private static readonly List<string> AdminEmails = new()
        {
            "admin1@lawapp.com",
            "kavindulakshan187@gmail.com"
        };
            
        public bool IsAdminEmail(string email)
        {
            return AdminEmails.Contains(email, StringComparer.OrdinalIgnoreCase);
        }
    }
}
