using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using MediatR;

namespace Aipazz.Application.Matters.matter.Queries
{
    public record GetMattersByMatterTypeIdQuery(string MatterTypeId, string UserId) : IRequest<List<Matter>>;
}
