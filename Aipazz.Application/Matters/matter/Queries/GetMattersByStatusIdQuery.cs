using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using MediatR;


namespace Aipazz.Application.Matters.matter.Queries
{
    public record GetMattersByStatusIdQuery(string StatusId, string UserId) : IRequest<List<Matter>>;
}

