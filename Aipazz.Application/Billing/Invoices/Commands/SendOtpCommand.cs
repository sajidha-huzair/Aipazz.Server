using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    public class SendOtpCommand : IRequest<bool>
    {
        public string Token { get; set; } = string.Empty;
    }
}
