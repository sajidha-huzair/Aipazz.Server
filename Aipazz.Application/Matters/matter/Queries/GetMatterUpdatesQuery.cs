using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matter.Queries
{
    public class GetMatterUpdatesQuery : IRequest<List<MatterUpdateHistory>>
    {
        public string MatterId { get; set; }
        public string ClientNic { get; set; }
        public string UserId { get; set; }
    }
}
