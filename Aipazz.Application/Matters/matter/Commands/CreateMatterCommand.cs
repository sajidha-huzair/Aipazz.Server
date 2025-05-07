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
        string ClientName,
        List<string> TeamMembers
    ) : IRequest<Matter>;
}
