using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    public class VerifyInvoiceLinkCommand : IRequest<VerifyLinkResult>
    {
        public string Token { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
