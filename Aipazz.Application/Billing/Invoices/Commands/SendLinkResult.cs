using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.matter.Commands
{
    namespace Aipazz.Application.Billing.Invoices.Commands
    {
        public class SendLinkResult
        {
            public bool Success { get; }
            public string Message { get; }

            public SendLinkResult(bool success, string message)
            {
                Success = success;
                Message = message;
            }
        }
    }

}
