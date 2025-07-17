using System.Collections.Generic;
using Aipazz.Domian.DocumentMgt;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Queries
{
    public record GetDocumentsByMatterIdQuery(string MatterId, string UserId) : IRequest<List<Document>>;
}