using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Application.Team.Queries;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class Chcekteamexsitshandler : IRequestHandler<CheckTeamExistsQuery, bool>
    {
        private readonly ITeamRepository _teamRepository;

        public Chcekteamexsitshandler(ITeamRepository teamRepository)
        {          
            _teamRepository = teamRepository;
        }
        public async Task<bool> Handle(CheckTeamExistsQuery request, CancellationToken cancellationToken)
        {
           return await _teamRepository.CheckTeamExistsAsync(request.TeamId);
        }
    }
}
