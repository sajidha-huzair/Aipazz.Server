using Aipazz.Domian.client;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Queries
{
    public class GetClientByIdQuery : IRequest<Client>
    {
        public string Id { get; }
        public string nic { get; }

        public GetClientByIdQuery(string id, string nic)
        {
            Id = id;
            this.nic = nic;
        }
    }
}
