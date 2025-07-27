using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Commands
{
    public class UpdateTokenCommand : IRequest<bool>
    {
        public string Token { get; set; }=string.Empty;
        public string? Otp { get; set; }
        public bool? IsUsed { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
