using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;
using MediatR;


namespace Aipazz.Application.Matters.matterStatus.Queries
{
    public record GetAllStatusesQuery : IRequest<List<Status>>;
}
