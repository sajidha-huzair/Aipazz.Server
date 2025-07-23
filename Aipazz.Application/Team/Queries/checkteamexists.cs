using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Aipazz.Application.Team.Queries
{
   public record CheckTeamExistsQuery(string TeamId) : IRequest<bool>;
}
