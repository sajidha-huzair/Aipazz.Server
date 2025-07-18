using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Aipazz.Application.Billing.DTOs;

namespace Aipazz.Application.Billing.Invoices.Commands
{

        public class VerifyTokenCommand : IRequest<VerifyTokenResult>
        {
            public string Token { get; }

            public VerifyTokenCommand(string token)
            {
                Token = token;
            }
        }
    }


