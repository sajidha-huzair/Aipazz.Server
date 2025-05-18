using Aipazz.Application.Billing.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Queries
{
    public class GetClientWithDetailsQuery : IRequest<ClientWithMattersDto>
    {
        public string ClientNic { get; set; }
        public string UserId { get; set; } 

        public GetClientWithDetailsQuery(string clientNic, string userId)
        {
            ClientNic = clientNic;
            UserId = userId;
        }
    }

}


