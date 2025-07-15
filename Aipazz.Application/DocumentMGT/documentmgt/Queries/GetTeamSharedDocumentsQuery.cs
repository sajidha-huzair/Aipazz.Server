using System.Collections.Generic;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Queries
{
    public record GetTeamSharedDocumentsQuery(string UserId) : IRequest<List<Document>>;
}