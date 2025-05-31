using MediatR;
using System;
using System.Collections.Generic;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.matter.Commands
{
    public record CreateMatterCommand(
        string Title,
        string CaseNumber,
        DateTime? Date,
        string Description,
        CourtType CourtType,
        string ClientNic,
        string StatusId,
        List<string> TeamMembers
    ) : IRequest<Matter>;
}
