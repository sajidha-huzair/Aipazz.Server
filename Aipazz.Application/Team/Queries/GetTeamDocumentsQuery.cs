using System.Collections.Generic;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.Team.Queries
{
    public record GetTeamDocumentsQuery(string TeamId) : IRequest<List<Document>>;
}
