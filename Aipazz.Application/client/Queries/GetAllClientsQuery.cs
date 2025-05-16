using Aipazz.Domian.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Aipazz.Domian.client;

namespace Aipazz.Application.client.Queries
{
  
        public record GetAllClientsQuery() : IRequest<List<Client>>;
    
}
