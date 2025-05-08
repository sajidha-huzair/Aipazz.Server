using Aipazz.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.client.Queries
{
    public class GetClientsWithEntriesQuery : IRequest<List<ClientWithMattersDto>> { }

}
